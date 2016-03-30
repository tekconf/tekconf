using System.Collections.Generic;

namespace Tekconf.DTO
{
    public class Speaker
    {
        public string FirstName { get; set; }

        public string MiddleName { get; set; }
		public string Slug {get;set;}
        public string LastName { get; set; }
        public string Bio { get; set; }
        public string ImageUrl { get; set; }
        public string WebsiteUrl { get; set; }
        public string JobTitle { get; set; }
        public string EmailAddress { get; set; }
        public string TwitterHandle { get; set; }
        public string CompanyName { get; set; }
        public string LinkedInUrl { get; set; }
        public string GithubUrl { get; set; }
        public string FacebookUrl { get; set; }
        public string PhoneNumber { get; set; }
    }
}