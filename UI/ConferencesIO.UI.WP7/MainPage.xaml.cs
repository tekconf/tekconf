using System.Windows;
using System.Windows.Controls;
using ConferencesIO.RemoteData;
using ConferencesIO.RemoteData.Dtos;
using ConferencesIO.RemoteData.Dtos.v1;
using ConferencesIO.RemoteData.v1;
using Microsoft.Phone.Controls;

namespace ConferencesIO.UI.WP7
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
      string baseUrl = "http://conferencesioapi.azurewebsites.net/";
      var client = new RemoteDataRepository(baseUrl);

      client.GetSessions("CodeMash-2012", sessions =>
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

    private void SessionSelected(object sender, SelectionChangedEventArgs e)
    {
      var conference = (ConferencesDto) e.AddedItems[ 0]; 
      MessageBox.Show( conference.name, "Full Conference", MessageBoxButton.OK);
    }

  }
}