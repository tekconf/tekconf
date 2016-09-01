﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TekConf.Api.Data.Models
{
    public class Tag : IEntity
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Slug { get; set; }


        [Required]
        [StringLength(1000)]
        public string Name { get; set; }

        #region Relationships
        public virtual ConferenceInstance ConferenceInstance { get; set; }
        public virtual ICollection<Session> Sessions { get; set; } = new List<Session>();

        #endregion
    }
}