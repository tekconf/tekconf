using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using TekConf.RemoteData.v1;
using System.Collections.Generic;
using TekConf.RemoteData.Dtos.v1;
using MonoTouch.Dialog.Utilities;
using MonoTouch.Dialog;

namespace TekConf.UI.iPhone
{
	public class SessionsListDialogViewController : BaseDialogViewController
	{
		public string SearchString { get; set; }
		
		public SessionsListDialogViewController () : base(UITableViewStyle.Plain, new RootElement("Sessions"), false)
		{
			this.EnableSearch = true;

			if (UIDevice.CurrentDevice.CheckSystemVersion (6, 0)) {
				RefreshControl = new UIRefreshControl ();
				RefreshControl.ValueChanged += (sender, e) => {
					Refresh (); 
				};
			} else {
				// old style refresh button
				NavigationItem.SetRightBarButtonItem (new UIBarButtonItem (UIBarButtonSystemItem.Refresh), false);
				NavigationItem.RightBarButtonItem.Clicked += (sender, e) => {
					Refresh (); 
				};
			}

		}
		
		public void Refresh ()
		{
			if (this.IsReachable ()) {
				var loading = new UIAlertView (" Downloading Sessions", "Please wait...", null, null, null);
				
				loading.Show ();
				
				var indicator = new UIActivityIndicatorView (UIActivityIndicatorViewStyle.WhiteLarge); 
				indicator.Center = new System.Drawing.PointF (loading.Bounds.Width / 2, loading.Bounds.Size.Height - 40); 
				indicator.StartAnimating (); 
				loading.AddSubview (indicator);

				Repository.GetSessions(NavigationItems.ConferenceSlug, sessions =>
				{
					if (sessions != null) {
						var rootElement = new RootElement ("Sessions"){ new Section() };

						foreach (SessionsDto session in sessions) {
							rootElement [0].Add (new SessionElement (session));
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
			
			TrackAnalyticsEvent ("SessionsListDialogViewController-" + NavigationItems.ConferenceSlug);
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
