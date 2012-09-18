using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Widget;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.Android
{
  public class SessionsListAdapter : BaseAdapter
  {
    readonly Activity _activity;
    private readonly IList<SessionsDto> _sessions;

    public SessionsListAdapter(Activity activity, IList<SessionsDto> sessions)
    {
      _activity = activity;
      _sessions = sessions;
    }

    public override int Count
    {
      get { return _sessions.Count; }
    }
    
    public override Java.Lang.Object GetItem(int position)
    {
      return null;
    }
    
    public override long GetItemId(int position)
    {
      return 0;
      //TODO : return _sessions[position].Id;
    }

    public override View GetView(int position, View convertView, ViewGroup parent)
    {
      var view = convertView ?? _activity.LayoutInflater.Inflate (Resource.Layout.SessionListItem, parent, false);
      var titleLabel = view.FindViewById<TextView>(Resource.Id.sessionTitleLabel);
      var startLabel = view.FindViewById<TextView>(Resource.Id.sessionStartLabel);

      startLabel.Text = _sessions[position].start.ToShortDateString();
      titleLabel.Text = _sessions[position].title;

      return view;
    }
  }
}