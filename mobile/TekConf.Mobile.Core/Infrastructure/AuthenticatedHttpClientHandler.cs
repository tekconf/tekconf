using System.Threading.Tasks;
using System.Net.Http;
using System.Threading;
using System.Net.Http.Headers;

namespace TekConf.Mobile.Core
{
	public class AuthenticatedHttpClientHandler : HttpClientHandler
	{
		private readonly string _token;

		public AuthenticatedHttpClientHandler(string token)
		{
			_token = token;
		}

		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			// See if the request has an authorize header
			var auth = request.Headers.Authorization;
			if (auth != null && !string.IsNullOrWhiteSpace(_token))
			{
				request.Headers.Authorization = new AuthenticationHeaderValue(auth.Scheme, _token);
			}
			else {
				request.Headers.Remove("Authorization");
			}

			return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
		}
	}
}