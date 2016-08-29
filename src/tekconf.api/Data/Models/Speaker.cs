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
        [StringLength(200)]
        public string Title { get; set; }

        [StringLength(200)]
        public string Company { get; set; }
        [StringLength(300)]
        public string BlogUrl { get; set; }
        [StringLength(200)]
        public string EmailAddress { get; set; }
        [StringLength(30)]
        public string PhoneNumber { get; set; }
        [StringLength(300)]
        public string ProfileImageUrl { get; set; }
        #region Social
        [StringLength(100)]
        public string TwitterName { get; set; }
        [StringLength(300)]
        public string FacebookUrl { get; set; }
        [StringLength(300)]
        public string LinkedInUrl { get; set; }

        [StringLength(300)]
        public string GooglePlusUrl { get; set; }
        [StringLength(300)]
        public string VimeoUrl { get; set; }
        [StringLength(300)]
        public string YoutubeUrl { get; set; }
        [StringLength(300)]
        public string GithubUrl { get; set; }
        [StringLength(300)]
        public string CoderWallUrl { get; set; }
        [StringLength(300)]
        public string StackoverflowUrl { get; set; }
        [StringLength(300)]
        public string BitbucketUrl { get; set; }
        [StringLength(300)]
        public string CodeplexUrl { get; set; }
        [StringLength(300)]
        public string InstagramUrl { get; set; }
        [StringLength(300)]
        public string SnapchatUrl { get; set; }
        #endregion

        #region Relationships
        public virtual ICollection<Session> Sessions { get; set; } = new List<Session>();
        public User User { get; set; }
        #endregion
    }
}