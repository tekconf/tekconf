using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using TekConf.RemoteData.v1;
using System.Collections.Generic;
using TekConf.RemoteData.Dtos.v1;
//using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;
namespace TekConf.UI.iPhone
{
	public class BaseUIViewController : UIViewController
	{
		private static UIFont _titleFont;
		public static UIFont TitleFont 
		{
			get
			{
				if (_titleFont == null)
				{
					_titleFont = UIFont.FromName ("OpenSans", 14f);
				}
				return _titleFont;
			}
		}

		private static UIFont _descriptionFont;
		public static UIFont DescriptionFont 
		{
			get
			{
				if (_descriptionFont == null)
				{
					_descriptionFont = UIFont.FromName ("OpenSans", 12f);
				}
				return _descriptionFont;
			}
		}

		//private static readonly MobileServiceClient MobileService = new MobileServiceClient ("https://tekconf.azure-mobile.net/");
		public BaseUIViewController (string nibName, NSBundle bundle) : base(nibName, bundle)
		{

		}

		protected void TrackAnalyticsEvent(string eventName)
		{
			FlurryAnalytics.FlurryAnalytics.LogEvent(eventName);

		}

		protected bool IsReachable()
		{
			return Reachability.IsHostReachable("api.tekconf.com");
		}

		private string _baseUrl = "http://api.tekconf.com";
		//private string _baseUrl = "http://192.168.1.116/TekConf.UI.Api";
		private RemoteDataRepository _client;
		protected RemoteDataRepository Repository
		{
			get
			{
				if (this._client == null)
				{
					this._client = new RemoteDataRepository(_baseUrl);
				}

				return this._client;
			}
		}

		protected UIAlertView UnreachableAlert()
		{
			return new UIAlertView("Unreachable", "Can not access TekConf.com. Check internet connection.", null, "OK", null);
		}

		protected static bool UserInterfaceIdiomIsPhone {
			get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad();

			//Task<MobileServiceUser> login = MobileService.LoginAsync("17351920-ZX9KsONzhX2uIGP54DEkY1D00Gu58fgTSzFLpgJ0");


			if (NavigationController != null)
			{
				//UIColor colorWithRed:0.506 green:0.6 blue:0.302 alpha:1
				NavigationController.NavigationBar.TintColor = UIColor.FromRGBA(red:0.506f, 
				                                                                green:0.6f, 
				                                                                blue:0.302f,
				                                                                alpha:1f);

			}

		}
	}
	
}
