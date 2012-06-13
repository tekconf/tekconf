using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace ArtekSoftware.Conference.Mobile.SL.Views
{
  public partial class Sessions : Page
  {
    public Sessions()
    {
      InitializeComponent();
    }

    // Executes when the user navigates to this page.
    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
      base.OnNavigatedTo(e);
      string baseUrl = "http://conference.azurewebsites.net/api/";
      var client = new RemoteData.Shared.RemoteData(baseUrl);
      client.GetConferences(conferences =>
      {
        //Deployment.Current.Dispatcher.BeginInvoke(() =>
        //{
          SessionsList.ItemsSource = conferences;
          //Loading.Visibility = Visibility.Collapsed;
        //});
      });
    }

  }
}
