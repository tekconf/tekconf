using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;

namespace ArtekSoftware.Conference.Mobile.WP7
{
  public partial class MainPage : PhoneApplicationPage
  {
    // Constructor
    public MainPage()
    {
      InitializeComponent();

      // Set the data context of the listbox control to the sample data
      //DataContext = App.ViewModel;
      //this.Loaded += new RoutedEventHandler(MainPage_Loaded);
    }

    // Load data for the ViewModel Items
    //private void MainPage_Loaded(object sender, RoutedEventArgs e)
    //{
    //  if (!App.ViewModel.IsDataLoaded)
    //  {
    //    App.ViewModel.LoadData();
    //  }
    //}

    protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
    {
      base.OnNavigatedTo(e);
      string baseUrl = "http://conference.azurewebsites.net/api/";
      var client = new RemoteData.Shared.RemoteData(baseUrl);
      client.GetConferences(conferences =>
      {
        Deployment.Current.Dispatcher.BeginInvoke(() =>
        {
          DataContext = conferences;
          Loading.Visibility = Visibility.Collapsed;
        });
      });
    }

    private void ConferenceSelected(object sender, SelectionChangedEventArgs e)
    {
      var conference = (RemoteData.Shared.Conference) e.AddedItems[ 0]; 
      MessageBox.Show( conference.Name, "Full Conference", MessageBoxButton.OK);
    }



  }
}