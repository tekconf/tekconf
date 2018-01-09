using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using tekconf.shared.Models;

namespace tekconf.web.Authorization
{
    public class ProposalApprovedAuthorizationHandler :
        AuthorizationHandler<ProposalRequirement, ProposalModel>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            ProposalRequirement requirement, ProposalModel resource)
        {
            if (!requirement.MustBeApproved)
                if (resource.Approved)
                    context.Fail();

            if (requirement.MustBeApproved)
                if (!resource.Approved)
                    context.Fail();

            return Task.CompletedTask;
        }
    }
}
