using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TekConf.Api.Data.Models
{
    public class Speaker : IEntity
    {
        public int Id { get; set; }

        [Index("IX_SpeakerSlug", order: 1, IsUnique = true)]
        [Required]
        [StringLength(200)]
        public string Slug { get; set; }

        [Required]
        [StringLength(200)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(200)]
        public string LastName { get; set; }
        public string Bio { get; set; }
        public string BlogUrl { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }

        #region Social
        public string TwitterName { get; set; }
        public string FacebookUrl { get; set; }
        public string LinkedInUrl { get; set; }
        public string ProfileImageUrl { get; set; }
        public string GooglePlusUrl { get; set; }
        public string VimeoUrl { get; set; }
        public string YoutubeUrl { get; set; }
        public string GithubUrl { get; set; }
        public string CoderWallUrl { get; set; }
        public string StackoverflowUrl { get; set; }
        public string BitbucketUrl { get; set; }
        public string CodeplexUrl { get; set; }
        public string InstagramUrl { get; set; }
        public string SnapchatUrl { get; set; }
        #endregion

        #region Relationships
        public virtual ICollection<Session> Sessions { get; set; } = new List<Session>();
        public User User { get; set; }
        #endregion
    }
}