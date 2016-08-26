using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TekConf.Api.Data.Models
{
    public class User : IEntity
    {
        public int Id { get; set; }


        [Index("IX_UserSlug", order: 1, IsUnique = true)]
        [Required]
        [StringLength(200)]
        public string Slug { get; set; }

        [Required]
        [StringLength(200)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(200)]
        public string LastName { get; set; }

        [Required]
        [StringLength(8000, MinimumLength = 20)]
        public string Bio { get; set; }

        #region Relationships

        public virtual ICollection<Presentation> Presentations { get; set; } = new List<Presentation>();
        public virtual ICollection<Conference> OwnedConferences { get; set; } = new List<Conference>();
        public virtual ICollection<Speaker> SpeakingEngagements { get; set; } = new List<Speaker>();

        #endregion
    }
}