using Android.App;
using Android.OS;
using Android.Widget;
using ConferencesIO.RemoteData.v1;

namespace ConferencesIO.Android
{
  [Activity(Label = "ConferencesIO.Android", MainLauncher = true, Icon = "@drawable/icon")]
  public class Activity1 : Activity
  {
    int count = 1;

    protected override void OnCreate(Bundle bundle)
    {
      base.OnCreate(bundle);

      // Set our view from the "main" layout resource
      SetContentView(Resource.Layout.Main);

      string baseUrl = "http://conferencesioapi.azurewebsites.net/v1/";
      //string baseUrl = "http://localhost/ConferencesIO.UI.Api/v1/";
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

