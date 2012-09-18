using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using TekConf.RemoteData.Dtos.v1;
using TekConf.RemoteData.v1;

namespace TekConf.UI.SL.Views
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
      string baseUrl = "http://api.tekconf.com/v1/";
      //string baseUrl = "http://localhost:25825/v1/";
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
