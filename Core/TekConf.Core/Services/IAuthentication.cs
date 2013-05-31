namespace TekConf.Core.Services
{
	public interface IAuthentication
	{
		bool IsAuthenticated { get; }
		//string UserId { get; }
		string OAuthProvider { get; }
		string UserName { get; set; }
	}
}