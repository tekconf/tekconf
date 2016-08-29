using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TekConf.Api.Data.Models
{
    public class Session : IEntity
    {

        public int Id { get; set; }


        [Index("IX_SessionSlug", order: 1, IsUnique = true)]
        [Required]
        [StringLength(200)]
        public string Slug { get; set; }

        [Required]
        [StringLength(500)]
        public string Title { get; set; }

        [Required]
        [StringLength(8000, MinimumLength = 20)]
        public string Description { get; set; }

        //[StringLength(255)]
        //public string LiveStreamUrl { get; set; }

        #region Relationships
        public ConferenceInstance ConferenceInstance { get; set; }
        public Presentation Presentation { get; set; }
        public virtual ICollection<Speaker> Speakers { get; set; } = new List<Speaker>();
        #endregion
    }
}