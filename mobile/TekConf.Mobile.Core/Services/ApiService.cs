using Refit;
using System;
using System.Net.Http;
using Fusillade;
using MvvmCross.Plugins.Messenger;
using TekConf.Mobile.Core.Messages;

namespace TekConf.Mobile.Core.Services
{
	public class ApiService : IApiService
	{
		public const string ApiBaseAddress = "https://tekauth.azurewebsites.net/api";
		private MvxSubscriptionToken token;
		private readonly ISettingsService _settingsService;
		readonly IMvxMessenger _messenger;

		public ApiService(ISettingsService settingsService, IMvxMessenger messenger)
		{
			_messenger = messenger;
			_settingsService = settingsService;
		    InitializeApis();
			token = _messenger.Subscribe<UserLoggedInMessage>(InitializeApis);
		}


		private Lazy<ITekConfApi> _background;
		private Lazy<ITekConfApi> _userInitiated;
		private Lazy<ITekConfApi> _speculative;

		private void InitializeApis(UserLoggedInMessage message)
		{
			InitializeApis();
		}

	    private void InitializeApis()
	    {
            Func<HttpMessageHandler, ITekConfApi> createClient = messageHandler =>
            {
                var client = new HttpClient(messageHandler)
                {
                    BaseAddress = new Uri(ApiBaseAddress)
                };

                return RestService.For<ITekConfApi>(client);
            };

            _background = new Lazy<ITekConfApi>(() => createClient(
                new RateLimitedHttpMessageHandler(new AuthenticatedHttpClientHandler(_settingsService.UserIdToken), Priority.Background)));

            _userInitiated = new Lazy<ITekConfApi>(() => createClient(
                new RateLimitedHttpMessageHandler(new AuthenticatedHttpClientHandler(_settingsService.UserIdToken), Priority.UserInitiated)));

            _speculative = new Lazy<ITekConfApi>(() => createClient(
                new RateLimitedHttpMessageHandler(new AuthenticatedHttpClientHandler(_settingsService.UserIdToken), Priority.Speculative)));

	        if (!string.IsNullOrWhiteSpace(_settingsService.UserIdToken))
	        {
				var authenticationMessage = new AuthenticationInitializedMessage(this);
				_messenger.Publish(authenticationMessage);
            }
        }

        public ITekConfApi Background
		{
			get { return _background.Value; }
		}

		public ITekConfApi UserInitiated
		{
			get { return _userInitiated.Value; }
		}

		public ITekConfApi Speculative
		{
			get { return _speculative.Value; }
		}
	}
}