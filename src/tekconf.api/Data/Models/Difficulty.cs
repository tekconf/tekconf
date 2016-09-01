using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TekConf.Api.Data.Models
{
    public class Difficulty : IEntity
    {
        public int Id { get; set; }

        [Required]
        [StringLength(1000)]
        public string Description { get; set; }
        public int Order { get; set; }
        
        #region Relationships
        public virtual ConferenceInstance ConferenceInstance { get; set; }
        public virtual ICollection<Session> Sessions { get; set; } = new List<Session>();

        #endregion
    }
}