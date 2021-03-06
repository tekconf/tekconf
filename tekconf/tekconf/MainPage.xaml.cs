﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using IdentityModel.OidcClient;
using IdentityModel.OidcClient.Browser;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;

namespace tekconf
{
	public partial class MainPage : ContentPage
	{
	    private readonly OidcClient _client;
	    private LoginResult _result;

	    private readonly Lazy<HttpClient> _apiClient = new Lazy<HttpClient>(() => new HttpClient());
	    private readonly string _tekconfApiUrl = "https://tekconfapi.azurewebsites.net/";
	    private readonly string _tekconfIdentityUrl = "https://tekconfidentity.azurewebsites.net";

        public MainPage()
        {
            InitializeComponent();

            Login.Clicked += Login_Clicked;
            CallApi.Clicked += CallApi_Clicked;

            var browser = DependencyService.Get<IBrowser>();

            var options = new OidcClientOptions
            {
                Authority = _tekconfIdentityUrl,
                ClientId = "com.arteksoftware.tekconf",
                //Scope = "openid profile email tekconfApi roles experience offline_access",
                //Scope = "openid profile email tekconfApi roles experience",
                Scope = "openid profile email",
                RedirectUri = "tekconfmobile://callback",
                Browser = browser,
                //Flow = OidcClientOptions.AuthenticationFlow.Hybrid,

                ResponseMode = OidcClientOptions.AuthorizeResponseMode.Redirect
            };

            _client = new OidcClient(options);
        }

        private async void Login_Clicked(object sender, EventArgs e)
        {
            _result = await _client.LoginAsync(new LoginRequest());

            if (_result.IsError)
            {
                OutputText.Text = _result.Error;
                return;
            }

            var sb = new StringBuilder(128);
            foreach (var claim in _result.User.Claims)
            {
                sb.AppendFormat("{0}: {1}\n", claim.Type, claim.Value);
            }

            sb.AppendFormat("\n{0}: {1}\n", "refresh token", _result?.RefreshToken ?? "none");
            sb.AppendFormat("\n{0}: {1}\n", "access token", _result.AccessToken);

            OutputText.Text = sb.ToString();

            _apiClient.Value.SetBearerToken(_result?.AccessToken ?? "");
            _apiClient.Value.BaseAddress = new Uri(_tekconfApiUrl);

        }

        private async void CallApi_Clicked(object sender, EventArgs e)
        {

            var result = await _apiClient.Value.GetAsync("conference/getall");

            if (result.IsSuccessStatusCode)
            {
                OutputText.Text = JArray.Parse(await result.Content.ReadAsStringAsync()).ToString();
            }
            else
            {
                OutputText.Text = result.ReasonPhrase;
            }
        }
	}
}
