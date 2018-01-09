﻿using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using tekconf.shared.Models;

namespace tekconf.web.Api
{
    public class AttendeeApiService
    {
        private readonly HttpClient client;
        private readonly IHttpContextAccessor httpContextAccessor;

        public AttendeeApiService(HttpClient client, IHttpContextAccessor httpContextAccessor)
        {
            this.client = client;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<AttendeeModel> GetById(int attendeeId)
        {
            AttendeeModel result = null;
            client.SetBearerToken(await httpContextAccessor.HttpContext.GetTokenAsync("access_token"));
            var response = await client.GetAsync($"/Attendee/GetById/{attendeeId}");
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsAsync<AttendeeModel>();
            }
            return result;
        }

        public async Task<AttendeeModel> Add(AttendeeModel attendee)
        {
            AttendeeModel result = null;
            client.SetBearerToken(await httpContextAccessor.HttpContext.GetTokenAsync("access_token"));
            var response = await client.PostAsJsonAsync("/Attendee/Add", attendee);
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsAsync<AttendeeModel>();
            }
            return result;
        }

        public async Task<int> GetAttendeesTotal(int conferenceId)
        {
            var result = 0;
            client.SetBearerToken(await httpContextAccessor.HttpContext.GetTokenAsync("access_token"));
            var response = await client.GetAsync($"/Attendee/GetAttendeesTotal/{conferenceId}");
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsAsync<int>();
            }
            return result;
        }
    }
}
