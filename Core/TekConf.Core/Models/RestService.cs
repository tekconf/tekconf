namespace TekConf.Core.Models
{
	using System.Net.Http;
	using System.Text;
	using System.Threading;
	using System.Threading.Tasks;
	using Cirrious.CrossCore.Platform;

	public class RestService : IRestService
	{
		private readonly IMvxJsonConverter _converter;

		public RestService(IMvxJsonConverter converter)
		{
			_converter = converter;
		}

		public async Task<T> GetAsync<T>(string url, CancellationToken cancellationToken) where T : new()
		{
			var client = new HttpClient();
			var response = await client.GetAsync(url, cancellationToken);
			var responseString = await response.Content.ReadAsStringAsync();

			if (!string.IsNullOrWhiteSpace(responseString))
			{
				var result = _converter.DeserializeObject<T>(responseString);
				return result;
			}

			return default(T);
		}

		public async Task<T> DeleteAsync<T>(string url, CancellationToken cancellationToken) where T : new()
		{
			var client = new HttpClient();
			var response = await client.DeleteAsync(url, cancellationToken);
			var responseString = await response.Content.ReadAsStringAsync();

			if (!string.IsNullOrWhiteSpace(responseString))
			{
				var result = _converter.DeserializeObject<T>(responseString);
				return result;
			}

			return default(T);
		}

		public async Task<T> PostAsync<T>(string url, object postContent, CancellationToken cancellationToken) where T : new()
		{
			var client = new HttpClient();
			var serializedContent = this._converter.SerializeObject(postContent);
			HttpContent httpContent = new StringContent(serializedContent, Encoding.UTF8, "application/json");

			var response = await client.PostAsync(url, httpContent, cancellationToken);
			var responseString = await response.Content.ReadAsStringAsync();

			if (!string.IsNullOrWhiteSpace(responseString))
			{
				var result = _converter.DeserializeObject<T>(responseString);
				return result;
			}

			return default(T);
		}
	}
}