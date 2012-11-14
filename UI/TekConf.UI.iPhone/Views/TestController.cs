using System;
using MonoTouch.Social;
using MonoTouch.UIKit;
using MonoTouch.Accounts;
using System.Linq;

namespace TekConf.UI.iPhone
{
	public class TestController : UIViewController
	{
		private ACAccountStore _accountStore;
		private string AppId = "417883241605228";

		public TestController ()
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			if (_accountStore == null)
				_accountStore = new ACAccountStore();
			
			var options = new AccountStoreOptions() {
				FacebookAppId = AppId
			};
			options.SetPermissions(ACFacebookAudience.OnlyMe, new string[]{ "email" });
			var accountType = _accountStore.FindAccountType(ACAccountType.Facebook);
			
			_accountStore.RequestAccess(accountType, options, (granted, error) => {
				
				// note: !mainthread
				
				if (granted) {
					
					var facebookAccount = _accountStore.FindAccounts(accountType).First();
					
					// success; do stuff with MonoTouch.Social, call their Graph API, etc.
					// access_token = facebookAccount.Credential.OAuthToken
					
					return;
				}
				
				if (error.Code == 7) {
					
					// cancel
					
					return;
				}
				
				// some other error, e.g. user has not set up his Facebook account in Settings
				// fallback; 1) Facebook app, 2) in-app UIWebView or Safari
				
			});
		}
	}
}

