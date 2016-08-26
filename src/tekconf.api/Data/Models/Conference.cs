using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TekConf.Api.Data.Models
{
    public class Conference : IEntity
    {
        public int Id { get; set; }

        [Index("IX_ConferenceSlug", order: 1, IsUnique = true)]
        [Required]
        [StringLength(200)]
        public string Slug { get; set; }

        [Index("IX_ConferenceName", order: 2, IsUnique = true)]
        [Required]
        [StringLength(300)]
        public string Name { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        [StringLength(8000, MinimumLength = 20)]
        public string Description { get; set; }

        #region Relationships
        public virtual ICollection<ConferenceInstance> Instances { get; set; } = new List<ConferenceInstance>();
        public User Owner { get; set; }
        
        #endregion
    }
}