
namespace TekConf.Mobile.Core.Services
{
	public class SettingsService : ISettingsService
	{
		public string UserIdToken { get; set; }
		public string EmailAddress {get;set;}
		public string Nickname {get;set;}

		private string _auth0Domain;

		public string Auth0Domain { 
			get {
				return "tekconf.auth0.com";
			}   
		}

		public string Auth0ClientId {
			get {
				return "XhxV5TtBdzUwth21O4jhvITp5I9hJ6xS";
			}
		}
	}
    
}