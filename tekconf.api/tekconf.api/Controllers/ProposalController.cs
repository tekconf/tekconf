﻿using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tekconf.api.Models;
using tekconf.api.Repositories;

namespace tekconf.api.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class ProposalController
    {
        private readonly ProposalRepo repo;

        public ProposalController(ProposalRepo repo)
        {
            this.repo = repo;
        }

        [HttpGet("GetAllApproved/{conferenceId}")]
        public IEnumerable<ProposalModel> GetAllApproved(int conferenceId)
        {
            return repo.GetAllApprovedForConference(conferenceId);
        }

        [HttpGet("GetAll/{conferenceId}")]
        public IEnumerable<ProposalModel> GetAll(int conferenceId)
        {
            return repo.GetAllForConference(conferenceId);
        }

        [HttpPost("Add")]
        public void Add([FromBody]ProposalModel model)
        {
            repo.Add(model);
        }

        [HttpGet("Approve/{proposalId}")]
        public ProposalModel Approve(int proposalId)
        {
            return repo.Approve(proposalId);
        }
    }
}
