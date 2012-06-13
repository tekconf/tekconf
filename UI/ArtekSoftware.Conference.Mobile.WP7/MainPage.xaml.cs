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
    }

    protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
    {
      base.OnNavigatedTo(e);
      string baseUrl = "http://conference.azurewebsites.net/api/";
      var client = new RemoteData.Shared.RemoteData(baseUrl);

      client.GetSessions("ThatConference-2012", sessions =>
                           {
                             Deployment.Current.Dispatcher.BeginInvoke(() =>
                             {
                               DataContext = sessions;
                               Loading.Visibility = Visibility.Collapsed;
                             });
                           });

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