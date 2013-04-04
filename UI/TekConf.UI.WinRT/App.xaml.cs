using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using TekConf.UI.WinRT.Common;
using System;
using System.Linq;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Popups;
// The Grid App template is documented at http://go.microsoft.com/fwlink/?LinkId=234226

namespace TekConf.UI.WinRT
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        public static MobileServiceClient MobileService = new MobileServiceClient(@"https://tekconfauth.azure-mobile.net/");
        /// <summary>
        /// Initializes the singleton Application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        public static bool IsAuthenticated { get; set; }
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
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {
            // Do not repeat app initialization when already running, just ensure that
            // the window is active
            if (args.PreviousExecutionState == ApplicationExecutionState.Running)
            {
                Window.Current.Activate();
                return;
            }
            await App.ViewModel.LoadConferences();
            // Create a Frame to act as the navigation context and associate it with
            // a SuspensionManager key
            var rootFrame = new Frame();
            SuspensionManager.RegisterFrame(rootFrame, "AppFrame");

            if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
            {
                // Restore the saved session state only when appropriate
                await SuspensionManager.RestoreAsync();
            }

            if (rootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                if (!rootFrame.Navigate(typeof(GroupedItemsPage), "AllGroups"))
                {
                    throw new Exception("Failed to create initial page");
                }
            }

            // Place the frame in the current Window and ensure that it is active
            Window.Current.Content = rootFrame;
            Window.Current.Activate();
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            await SuspensionManager.SaveAsync();
            deferral.Complete();
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
                            //conference.imageUrl = "http://www.tekconf.com" + conference.imageUrl;
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
                //var url = "http://api.tekconf.com/v1/conferences?showPastConferences=true&format=json";
                var url = "http://localhost:25825/v1/conferences?showPastConferences=false&format=json";
                var client = new HttpClient();
                var response = await client.GetAsync(url);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    // parse to json
                    var conferences = JsonConvert.DeserializeObject<List<ConferencesDto>>(responseString);
                    if (conferences != null)
                    {
                        foreach (var conference in conferences)
                        {
                            this.Items.Add(conference);
                        }
                    }
                }
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
