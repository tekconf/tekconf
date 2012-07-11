using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using ConferencesIO.RemoteData;
using ConferencesIO.RemoteData.Dtos;
using ConferencesIO.RemoteData.Dtos.v1;
using ConferencesIO.RemoteData.v1;

namespace ConferencesIO.UI.SL.Views
{
  public partial class Sessions : Page
  {
    public Sessions()
    {
      InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
      base.OnNavigatedTo(e);
      string baseUrl = "http://conferencesioapi.azurewebsites.net/v1/";
      //string baseUrl = "http://localhost/ConferencesIO.UI.Api/v1/";
      var client = new RemoteDataRepository(baseUrl);
      client.GetSessions("CodeMash-2012", sessions =>
      {
        Deployment.Current.Dispatcher.BeginInvoke(() =>
        {
          DataContext = sessions;
          //Loading.Visibility = Visibility.Collapsed;
        });
      });
    }

    private void ConferenceSelected(object sender, SelectionChangedEventArgs e)
    {
      var session = (SessionsDto)e.AddedItems[0];
      MessageBox.Show(session.title, "Full Session", MessageBoxButton.OK);
    }

  }
}
