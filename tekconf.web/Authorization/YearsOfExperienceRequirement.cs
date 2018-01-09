
using Microsoft.AspNetCore.Authorization;

namespace tekconf.web.Authorization
{
    public class YearsOfExperienceRequirement : IAuthorizationRequirement
    {
        public YearsOfExperienceRequirement(int yearsOfExperienceRequired)
        {
            YearsOfExperienceRequired = yearsOfExperienceRequired;
        }
        public int YearsOfExperienceRequired { get; set; }
    }
}
