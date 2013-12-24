namespace TekConf.Common.Entities
{
	public interface IAspNetUsersRepository : IRepository<AspNetUser>
	{
		string GetUserName(string providerName, string providerKey);
	}
}