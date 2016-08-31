using System;
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

        //TODO : Add Room, Dates, etc
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public string Building { get; set; }
        public string Room { get; set; }

        //public string Difficulty { get; set; }
        public string TwitterHashTag { get; set; }
        //public string SessionType { get; set; }
        //public bool IsAddedToSchedule { get; set; }
        //public List<string> Links { get; set; }
        //public List<string> Tags { get; set; }
        //public List<string> Subjects { get; set; }
        //public List<string> Resources { get; set; }
        //public List<string> Prerequisites { get; set; }




        #region Relationships
        public ConferenceInstance ConferenceInstance { get; set; }
        public Presentation Presentation { get; set; }
        public virtual ICollection<Speaker> Speakers { get; set; } = new List<Speaker>();
        public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();

        #endregion
    }
}