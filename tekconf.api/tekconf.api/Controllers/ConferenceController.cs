using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tekconf.api.Models;
using tekconf.api.Repositories;

namespace tekconf.api.Controllers
{
    [Authorize]
    public class ConferenceController : Controller
    {
        private readonly ConferenceRepo repo;

        public ConferenceController(ConferenceRepo repo)
        {
            this.repo = repo;
        }

        public IEnumerable<ConferenceModel> GetAll()
        {
            return repo.GetAll();
        }

        [HttpPost]
        public void Add([FromBody]ConferenceModel conference)
        {
            repo.Add(conference);
        }
    }
}
