using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TekConf.Api.Data.Models
{
    public class Presentation : IEntity
    {
        public int Id { get; set; }


        [Index("IX_PresentationSlug", order: 1, IsUnique = true)]
        [Required]
        [StringLength(200)]
        public string Slug { get; set; }

        [Required]
        [StringLength(500)]
        public string Title { get; set; }

        [Required]
        [StringLength(8000, MinimumLength = 20)]
        public string Description { get; set; }

        #region Relationships
        public virtual ICollection<Session> Sessions { get; set; } = new List<Session>();
        public virtual ICollection<User> Owners { get; set; } = new List<User>();
        #endregion

    }
}