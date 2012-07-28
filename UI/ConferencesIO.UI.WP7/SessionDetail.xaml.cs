using System.Windows;
using ConferencesIO.RemoteData.v1;
using Microsoft.Phone.Controls;

namespace ConferencesIO.UI.WP7
{
  public partial class SessionDetail : PhoneApplicationPage
  {
    public SessionDetail()
    {
      InitializeComponent();
    }
    protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
    {
      base.OnNavigatedTo(e);
     
      var conferenceSlug = NavigationContext.QueryString["conferenceSlug"];
      var sessionSlug = NavigationContext.QueryString["sessionSlug"];

      string baseUrl = "http://conferencesioapi.azurewebsites.net/v1/";
      var client = new RemoteDataRepository(baseUrl);

      client.GetSession("CodeMash-2012", sessionSlug, session =>
      {
        Deployment.Current.Dispatcher.BeginInvoke(() =>
        {
          DataContext = session;
          Loading.Visibility = Visibility.Collapsed;
        });
      });

      this.ApplicationTitle.Text = "CodeMash";
    }
  }
}