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
	public class ConferencesDialogViewController : BaseDialogViewController
	{
		private bool _showPastConferences = false;

		public string SearchString { get; set; }

		public ConferencesDialogViewController () : base(UITableViewStyle.Plain, new RootElement("Conferences"), false)
		{
			this.EnableSearch = true;

			if (NavigationItem != null) {

				var pastButton = new UIBarButtonItem () { Title = "Past" };

				pastButton.Clicked += (sender, e) => {
					_showPastConferences = !_showPastConferences; 
					Refresh ();
					if (_showPastConferences) {
						NavigationItem.RightBarButtonItem.Title = "Current";
					} else {
						NavigationItem.RightBarButtonItem.Title = "Past";
					}
				};
				NavigationItem.SetRightBarButtonItem (pastButton, false);
			}

			if (UIDevice.CurrentDevice.CheckSystemVersion (6, 0)) {
				RefreshControl = new UIRefreshControl ();
				RefreshControl.ValueChanged += (sender, e) => {
					Refresh (); };
			} else {
				// old style refresh button
				NavigationItem.SetRightBarButtonItem (new UIBarButtonItem (UIBarButtonSystemItem.Refresh), false);
				NavigationItem.RightBarButtonItem.Clicked += (sender, e) => {
					Refresh (); };
			}

			Refresh ();
		}

		public override void LoadView ()
		{
			base.LoadView ();

			this.View = this.TableView;
			if (ParentViewController != null && ParentViewController.View != null) {
				ParentViewController.View.BackgroundColor = UIColor.Red;
			}
		}

		public void Refresh ()
		{
			if (this.IsReachable ()) {
				var loading = new UIAlertView (" Downloading Conferences", "Please wait...", null, null, null);
				
				loading.Show ();
				
				var indicator = new UIActivityIndicatorView (UIActivityIndicatorViewStyle.WhiteLarge); 
				indicator.Center = new System.Drawing.PointF (loading.Bounds.Width / 2, loading.Bounds.Size.Height - 40); 
				indicator.StartAnimating (); 
				loading.AddSubview (indicator);

				Repository.GetConferences (sortBy: "", showPastConferences: _showPastConferences, search: "", callback: conferences => 
				{ 
					if (conferences != null) {
						var rootElement = new RootElement ("Conferences"){ new Section() };
	
						UIImage defaultImage = UIImage.FromBundle (@"images/DefaultConference.png");

						foreach (var conference in conferences) {
							rootElement [0].Add (new ConferenceElement (conference, defaultImage));
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

			TrackAnalyticsEvent ("ConferencesDialogViewController");
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

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