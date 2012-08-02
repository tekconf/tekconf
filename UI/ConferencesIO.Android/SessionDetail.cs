using System;
using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Widget;

namespace ConferencesIO.AndroidApp
{
  //[Activity(Label = "SessionDetail")]
  //public class SessionDetail : ListActivity
  //{
  //  List<Tuple<string, string>> items;

  //  protected override void OnCreate(Bundle bundle)
  //  {
  //    base.OnCreate(bundle);

  //    items = new List<Tuple<string, string>>();
  //    items.Add(new Tuple<string, string>("Fiji", "A nice beach"));
  //    items.Add(new Tuple<string, string>("Beijing", "AKA Shanghai"));
  //    items.Add(new Tuple<string, string>("Seedlings", "Tiny plants"));
  //    items.Add(new Tuple<string, string>("Plants", "Green plants"));

  //    ListAdapter = new SessionDetailAdapter(this, items);
  //  }

  //  protected override void OnListItemClick(Android.Widget.ListView l, Android.Views.View v, int position, long id)
  //  {
  //    var t = items[position];
  //    Android.Widget.Toast.MakeText(this, t.Item1, Android.Widget.ToastLength.Short).Show();
  //    Console.WriteLine("Clicked on " + t.Item1);
  //  }
  //}
}