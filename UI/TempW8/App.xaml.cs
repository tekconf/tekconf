using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Resources;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Newtonsoft.Json;
using TempW8.Resources;
using TempW8.ViewModels;

namespace TempW8
{
    public partial class App : Application
    {
        //private static MainViewModel viewModel = null;

        ///// <summary>
        ///// A static ViewModel used by the views to bind against.
        ///// </summary>
        ///// <returns>The MainViewModel object.</returns>
        //public static MainViewModel ViewModel
        //{
        //    get
        //    {
        //        // Delay creation of the view model until necessary
        //        if (viewModel == null)
        //            viewModel = new MainViewModel();

        //        return viewModel;
        //    }
        //}

        private static ConferencesViewModel _viewModel;
        public static ConferencesViewModel ViewModel
        {
            get
            {
                if (_viewModel == null)
                    _viewModel = new ConferencesViewModel();
                return _viewModel;
            }
        }

        /// <summary>
        /// Provides easy access to the root frame of the Phone Application.
        /// </summary>
        /// <returns>The root frame of the Phone Application.</returns>
        public static PhoneApplicationFrame RootFrame { get; private set; }

        /// <summary>
        /// Constructor for the Application object.
        /// </summary>
        public App()
        {
            // Global handler for uncaught exceptions.
            UnhandledException += Application_UnhandledException;

            // Standard XAML initialization
            InitializeComponent();

            // Phone-specific initialization
            InitializePhoneApplication();

            // Language display initialization
            InitializeLanguage();

            // Show graphics profiling information while debugging.
            if (Debugger.IsAttached)
            {
                // Display the current frame rate counters
                Application.Current.Host.Settings.EnableFrameRateCounter = true;

                // Show the areas of the app that are being redrawn in each frame.
                //Application.Current.Host.Settings.EnableRedrawRegions = true;

                // Enable non-production analysis visualization mode,
                // which shows areas of a page that are handed off to GPU with a colored overlay.
                //Application.Current.Host.Settings.EnableCacheVisualization = true;

                // Prevent the screen from turning off while under the debugger by disabling
                // the application's idle detection.
                // Caution:- Use this under debug mode only. Application that disables user idle detection will continue to run
                // and consume battery power when the user is not using the phone.
                PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
            }
        }



        // Code to execute when the application is launching (eg, from Start)
        // This code will not execute when the application is reactivated
        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
            
        }

        // Code to execute when the application is activated (brought to foreground)
        // This code will not execute when the application is first launched
        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
            App.ViewModel.LoadConferences();
            //// Ensure that application state is restored appropriately
            //if (!App.ViewModel.IsDataLoaded)
            //{
            //    App.ViewModel.LoadData();
            //}
        }

        // Code to execute when the application is deactivated (sent to background)
        // This code will not execute when the application is closing
        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {
        }

        // Code to execute when the application is closing (eg, user hit Back)
        // This code will not execute when the application is deactivated
        private void Application_Closing(object sender, ClosingEventArgs e)
        {
            // Ensure that required application state is persisted here.
        }

        // Code to execute if a navigation fails
        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                // A navigation has failed; break into the debugger
                Debugger.Break();
            }
        }

        // Code to execute on Unhandled Exceptions
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                Debugger.Break();
            }
        }

        #region Phone application initialization

        // Avoid double-initialization
        private bool phoneApplicationInitialized = false;

        // Do not add any additional code to this method
        private void InitializePhoneApplication()
        {
            if (phoneApplicationInitialized)
                return;

            // Create the frame but don't set it as RootVisual yet; this allows the splash
            // screen to remain active until the application is ready to render.
            RootFrame = new PhoneApplicationFrame();
            RootFrame.Navigated += CompleteInitializePhoneApplication;

            // Handle navigation failures
            RootFrame.NavigationFailed += RootFrame_NavigationFailed;

            // Handle reset requests for clearing the backstack
            RootFrame.Navigated += CheckForResetNavigation;

            // Ensure we don't initialize again
            phoneApplicationInitialized = true;
        }

        // Do not add any additional code to this method
        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            // Set the root visual to allow the application to render
            if (RootVisual != RootFrame)
                RootVisual = RootFrame;

            // Remove this handler since it is no longer needed
            RootFrame.Navigated -= CompleteInitializePhoneApplication;
        }

        private void CheckForResetNavigation(object sender, NavigationEventArgs e)
        {
            // If the app has received a 'reset' navigation, then we need to check
            // on the next navigation to see if the page stack should be reset
            if (e.NavigationMode == NavigationMode.Reset)
                RootFrame.Navigated += ClearBackStackAfterReset;
        }

        private void ClearBackStackAfterReset(object sender, NavigationEventArgs e)
        {
            // Unregister the event so it doesn't get called again
            RootFrame.Navigated -= ClearBackStackAfterReset;

            // Only clear the stack for 'new' (forward) and 'refresh' navigations
            if (e.NavigationMode != NavigationMode.New && e.NavigationMode != NavigationMode.Refresh)
                return;

            // For UI consistency, clear the entire page stack
            while (RootFrame.RemoveBackEntry() != null)
            {
                ; // do nothing
            }
        }

        #endregion

        // Initialize the app's font and flow direction as defined in its localized resource strings.
        //
        // To ensure that the font of your application is aligned with its supported languages and that the
        // FlowDirection for each of those languages follows its traditional direction, ResourceLanguage
        // and ResourceFlowDirection should be initialized in each resx file to match these values with that
        // file's culture. For example:
        //
        // AppResources.es-ES.resx
        //    ResourceLanguage's value should be "es-ES"
        //    ResourceFlowDirection's value should be "LeftToRight"
        //
        // AppResources.ar-SA.resx
        //     ResourceLanguage's value should be "ar-SA"
        //     ResourceFlowDirection's value should be "RightToLeft"
        //
        // For more info on localizing Windows Phone apps see http://go.microsoft.com/fwlink/?LinkId=262072.
        //
        private void InitializeLanguage()
        {
            try
            {
                // Set the font to match the display language defined by the
                // ResourceLanguage resource string for each supported language.
                //
                // Fall back to the font of the neutral language if the Display
                // language of the phone is not supported.
                //
                // If a compiler error is hit then ResourceLanguage is missing from
                // the resource file.
                RootFrame.Language = XmlLanguage.GetLanguage(AppResources.ResourceLanguage);

                // Set the FlowDirection of all elements under the root frame based
                // on the ResourceFlowDirection resource string for each
                // supported language.
                //
                // If a compiler error is hit then ResourceFlowDirection is missing from
                // the resource file.
                FlowDirection flow = (FlowDirection)Enum.Parse(typeof(FlowDirection), AppResources.ResourceFlowDirection);
                RootFrame.FlowDirection = flow;
            }
            catch
            {
                // If an exception is caught here it is most likely due to either
                // ResourceLangauge not being correctly set to a supported language
                // code or ResourceFlowDirection is set to a value other than LeftToRight
                // or RightToLeft.

                if (Debugger.IsAttached)
                {
                    Debugger.Break();
                }

                throw;
            }
        }
    }

    public class ConferencesViewModel
    {
        public ConferencesViewModel()
        {
            this.Items = new ObservableCollection<ConferencesDto>();
        }

        public ObservableCollection<ConferencesDto> Items { get; private set; }

        public List<SomeShit> GroupedItems
        {
            get
            {
                var groups = App.ViewModel.Items
                            .GroupBy(c => c.start.ToString("MMMM, yyyy"))
                            .Distinct()
                            .Select(g => new SomeShit()
                            {
                                GroupMonthName = g.Key,
                                Conferences = g.ToList()
                            })
                            .ToList();
                foreach (var someShit in groups)
                {
                    foreach (var conference in someShit.Conferences)
                    {
                        if (!string.IsNullOrWhiteSpace(conference.imageUrl))
                        {
                            conference.imageUrl = "http://www.tekconf.com" + conference.imageUrl;
                        }
                    }
                }

                return groups;
            }
        }

        public Task LoadConferences()
        {
            return Task.Run(async () =>
            {
                var url = "http://api.tekconf.com/v1/conferences?showPastConferences=true&format=json";
                //var client = new HttpClient();
                //var response = await client.GetAsync(url);

                WebClient client = new WebClient();
                //string s = await client.DownloadStringTaskAsync(url);
    
                //if (response.StatusCode == HttpStatusCode.OK)
                //{
                    var responseString = await client.DownloadStringTaskAsync(url);
                    // parse to json
                    var conferences = JsonConvert.DeserializeObject<List<ConferencesDto>>(responseString);
                    if (conferences != null)
                    {
                        foreach (var conference in conferences)
                        {
                            this.Items.Add(conference);
                        }
                    }
                //}
            });

        }
    }


    public static class Helpers
    {
        public static string GenerateSlug(this string phrase)
        {
            string slug = phrase.ToLower();
            //string slug = phrase.RemoveAccent().ToLower();
            // invalid chars           
            slug = Regex.Replace(slug, @"[^a-z0-9\s-]", "");
            // convert multiple spaces into one space   
            slug = Regex.Replace(slug, @"\s+", " ").Trim();
            // cut and trim 
            slug = slug.Substring(0, slug.Length <= 45 ? slug.Length : 45).Trim();
            slug = Regex.Replace(slug, @"\s", "-"); // hyphens   
            return slug;
        }

        //public static string RemoveAccent(this string txt)
        // {
        //byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(txt);
        //return System.Text.Encoding.UTF8.GetString(bytes);
        //}
    }

    public class ConferencesDto
    {
        public string name { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public string location { get; set; }
        public AddressDto address { get; set; }
        public string url { get; set; }
        public string description { get; set; }
        public string imageUrl { get; set; }
        public DateTime registrationOpens { get; set; }
        public DateTime registrationCloses { get; set; }
        public string slug
        {
            get { return name.GenerateSlug(); }
        }
        public bool IsOnSale()
        {
            bool isOnSale = this.registrationOpens <= DateTime.Now && this.registrationCloses >= DateTime.Now;

            return isOnSale;
        }
        public string CalculateConferenceDates(ConferencesDto conference)
        {
            string conferenceDates = "No dates scheduled";
            if (conference.start != default(DateTime) && conference.end != default(DateTime))
            {
                if (conference.start.Date == conference.end.Date)
                {
                    conferenceDates = conference.start.ToString("MMMM d, yyyy");
                }
                else if (conference.start.Year == conference.end.Year)
                {
                    if (conference.start.Month == conference.end.Month)
                    {
                        //@startDate.ToString("MMMM")<text> </text>@startDate.Day<text> - </text>@endDate.Day<text>, </text>@startDate.Year
                        conferenceDates = conference.start.ToString("MMMM d") + " - " + conference.end.Day + ", " + conference.end.Year;
                    }
                    else
                    {
                        conferenceDates = conference.start.ToString("MMMM d") + " - " + conference.end.ToString("MMMM d") + ", " + conference.end.Year;
                    }
                }
                else
                {
                    conferenceDates = conference.start.ToString("MMMM d, yyyy") + " - " + conference.end.ToString("MMMM d, yyyy");
                }
            }

            return conferenceDates;
        }
    }

    public class AddressDto
    {
        public int StreetNumber { get; set; }
        public string BuildingName { get; set; }
        public string StreetNumberSuffix { get; set; }
        public string StreetName { get; set; }
        public string StreetType { get; set; }
        public string StreetDirection { get; set; }
        public string AddressType { get; set; }
        public string AddressTypeId { get; set; }
        public string LocalMunicipality { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string GoverningDistrict { get; set; }
        public string PostalArea { get; set; }
        public string Country { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }


}