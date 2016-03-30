
namespace TekConf.Mobile.Core.Services
{
	public interface ISettingsService
	{
		string UserIdToken { get; set; }
		string Auth0Domain { get;  }
		string Auth0ClientId { get;  }
		string EmailAddress {get;set;}
		string Nickname {get;set;}
	}

}