using System;
using System.Windows;
using System.Windows.Controls;
using TekConf.RemoteData.Dtos.v1;
using TekConf.RemoteData.v1;
using Microsoft.Phone.Controls;

namespace TekConf.UI.WP7
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
      string baseUrl = "http://api.tekconf.com/v1/";
      var client = new RemoteDataRepository(baseUrl);

      client.GetSessions("CodeMash-2012", sessions =>
                           {
                             Deployment.Current.Dispatcher.BeginInvoke(() =>
                             {
                               DataContext = sessions;
                               Loading.Visibility = Visibility.Collapsed;
                             });
                           });
    }

    private void SessionSelected(object sender, SelectionChangedEventArgs e)
    {
      if (this.Items.SelectedIndex == -1)
      {
        return;
      }
      var session = (SessionsDto) e.AddedItems[0];
      //NavigationService.Navigate(new Uri("/SessionDetail.xaml?selectedItem=" + ((SessionViewModel)MainListBox.SelectedItem).Uri, UriKind.Relative));
      var url = string.Format("/SessionDetail.xaml?conferenceSlug={0}&sessionSlug={1}", "CodeMash-2012", session.slug);
      NavigationService.Navigate(new Uri(url, UriKind.Relative));

      //MessageBox.Show( session.slug, "Full Conference", MessageBoxButton.OK);
      this.Items.SelectedIndex = -1;
    }

  }
}