using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using TekConf.RemoteData.v1;
using System.Collections.Generic;
using TekConf.RemoteData.Dtos.v1;
using MonoTouch.Dialog.Utilities;
using MonoTouch.Dialog;
using System.Linq;

namespace TekConf.UI.iPhone
{
	public class SpeakersListDialogViewController : BaseDialogViewController
	{
		public string SearchString { get; set; }
		
		public SpeakersListDialogViewController () : base(UITableViewStyle.Plain, new RootElement("Speakers"), false)
		{
			this.EnableSearch = true;
			
			if (UIDevice.CurrentDevice.CheckSystemVersion (6, 0)) {
				RefreshControl = new UIRefreshControl ();
				RefreshControl.ValueChanged += (sender, e) => {
					Refresh (); 
				};
			} else {
				NavigationItem.SetRightBarButtonItem (new UIBarButtonItem (UIBarButtonSystemItem.Refresh), false);
				NavigationItem.RightBarButtonItem.Clicked += (sender, e) => {
					Refresh (); 
				};
			}
			
		}
		
		public void Refresh ()
		{
			if (this.IsReachable ()) {
				var loading = new UIAlertView (" Downloading Speakers", "Please wait...", null, null, null);
				
				loading.Show ();
				
				var indicator = new UIActivityIndicatorView (UIActivityIndicatorViewStyle.WhiteLarge); 
				indicator.Center = new System.Drawing.PointF (loading.Bounds.Width / 2, loading.Bounds.Size.Height - 40); 
				indicator.StartAnimating (); 
				loading.AddSubview (indicator);
				
				Repository.GetSpeakers(NavigationItems.ConferenceSlug, speakers =>
				{
					if (speakers != null) {
						speakers = speakers.OrderBy(s => s.lastName).ToList();
						var rootElement = new RootElement ("Speakers"){ new Section() };

						foreach (var speaker in speakers) {
							rootElement [0].Add (new SpeakerElement (speaker));
							//rootElement [0].Add (new StringElement (speaker.fullName));
						
						}
						
						InvokeOnMainThread (() => 
						{ 
							Root = rootElement;
							this.ReloadData ();
							this.TableView.ScrollsToTop = true;
							
							loading.DismissWithClickedButtonIndex (0, true);
							
							if (UIDevice.CurrentDevice.CheckSystemVersion (6, 0)) {
								RefreshControl.EndRefreshing ();
							}
							
						});
					}
				});
			} else {
				UnreachableAlert ().Show ();
			}
			
			TrackAnalyticsEvent ("SpeakersListDialogViewController-" + NavigationItems.ConferenceSlug);
		}
		
		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			
			Refresh ();
			if (!string.IsNullOrEmpty (SearchString)) {
				this.PerformFilter (SearchString);
			}
			
		}
		
		public override void FinishSearch ()
		{
			base.FinishSearch ();
		}
		
		public override void OnSearchTextChanged (string text)
		{
			base.OnSearchTextChanged (text);
			SearchString = text;
			TableView.SetNeedsDisplay ();
		}
	}
	
}
