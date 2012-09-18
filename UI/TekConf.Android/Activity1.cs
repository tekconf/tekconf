using Android.App;
using Android.OS;
using Android.Widget;
using TekConf.RemoteData.v1;

namespace TekConf.Android
{
    [Activity(Label = "TekConf.Android", MainLauncher = true, Icon = "@drawable/icon")]
    public class Activity1 : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            string baseUrl = "http://api.tekconf.com/v1/";
            //string baseUrl = "http://localhost:25825/v1/";
            var client = new RemoteDataRepository(baseUrl);
            var loading = ProgressDialog.Show(this, "Downloading Sessions", "Please wait...", true);
            client.GetSessions("CodeMash-2012", sessions =>
            {
                RunOnUiThread(() =>
                  {
                      var contactsAdapter = new SessionsListAdapter(this, sessions);
                      var contactsListView = FindViewById<ListView>(Resource.Id.sessionsListView);
                      contactsListView.Adapter = contactsAdapter;

                      contactsListView.ItemClick += (sender, args) =>
                                                      {
                                                          var selectedSession = sessions[args.Position];
                                                          new AlertDialog.Builder(this)
                                                            .SetTitle("Full Session")
                                                            .SetMessage(selectedSession.title)
                                                            .SetPositiveButton("Ok", delegate { })
                                                            .Show();
                                                      };

                      loading.Hide();

                  }
                );
            });



        }
    }
}

