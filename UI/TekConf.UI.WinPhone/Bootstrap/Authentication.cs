using TekConf.Core.Services;

namespace TekConf.UI.WinPhone.Bootstrap
{
	public class Authentication : IAuthentication
	{
		public bool IsAuthenticated { 
			get
			{
				return App.MobileService.CurrentUser != null;
			}
		}
		public string UserId { 
			get
			{
				if (IsAuthenticated)
					return App.MobileService.CurrentUser.UserId.Split(':')[1];
				else
					return string.Empty;
			} 
		}

		public string OAuthProvider
		{
			get
			{
				if (IsAuthenticated)
					return App.MobileService.CurrentUser.UserId.Split(':')[0];
				else
					return string.Empty;
			}
		}
		public string UserName { get; set; }
	}
}
