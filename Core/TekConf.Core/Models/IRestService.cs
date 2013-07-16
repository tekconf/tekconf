namespace TekConf.Core.Models
{
	using System.Threading;
	using System.Threading.Tasks;

	public interface IRestService
	{
		Task<T> GetAsync<T>(string url, CancellationToken cancellationToken) where T : new();

		Task<T> PostAsync<T>(string url, object postContent, CancellationToken cancellationToken) where T : new();
	}
}