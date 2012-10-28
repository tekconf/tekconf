using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using TekConf.RemoteData.v1;
using System.Collections.Generic;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.UI.iPhone
{
	public class BaseUIViewController : UIViewController
	{
		public BaseUIViewController (string nibName, NSBundle bundle) : base(nibName, bundle)
		{
			
		}

		private string _baseUrl = "http://api.tekconf.com";
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

		protected static bool UserInterfaceIdiomIsPhone {
			get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
		}
	}
	
}
