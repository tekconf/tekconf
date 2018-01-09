using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using tekconf.shared.Models;
using System.Collections.Generic;

namespace tekconf.web.Api
{
    public class ProposalApiService
    {
        private readonly HttpClient client;
        private readonly IHttpContextAccessor httpContextAccessor;

        public ProposalApiService(HttpClient client, IHttpContextAccessor httpContextAccessor)
        {
            this.client = client;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<ProposalModel>> GetAllForConference(int conferenceId)
        {
            var result = new List<ProposalModel>();
            client.SetBearerToken(await httpContextAccessor.HttpContext.GetTokenAsync("access_token"));
            var response = await client.GetAsync($"/Proposal/GetAll/{conferenceId}");
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsAsync<List<ProposalModel>>();
            }
            return result;
        }

        public async Task<IEnumerable<ProposalModel>> GetAllApprovedForConference(int conferenceId)
        {
            var result = new List<ProposalModel>();
            client.SetBearerToken(await httpContextAccessor.HttpContext.GetTokenAsync("access_token"));
            var response = await client.GetAsync($"/Proposal/GetAllApprovedForConference/{conferenceId}");
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsAsync<List<ProposalModel>>();
            }
            return result;
        }

        public async Task Add(ProposalModel model)
        {
            client.SetBearerToken(await httpContextAccessor.HttpContext.GetTokenAsync("access_token"));
            var response = await client.PostAsJsonAsync("/Proposal/Add/", model);
        }

        public async Task<ProposalModel> Approve(int proposalId)
        {
            var result = new ProposalModel();
            client.SetBearerToken(await httpContextAccessor.HttpContext.GetTokenAsync("access_token"));
            var response = await client.GetAsync($"/Proposal/Approve/{proposalId}");
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsAsync<ProposalModel>();
            }
            return result;
        }
    }
}
