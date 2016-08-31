using System.Collections.Generic;

namespace TekConf.Api.Data.Models
{
    public class Tag : IEntity
    {
        public int Id { get; set; }

        public string Slug { get; set; }
        public string Name { get; set; }

        #region Relationships
        public virtual ConferenceInstance ConferenceInstance { get; set; }
        public virtual ICollection<Session> Sessions { get; set; } = new List<Session>();

        #endregion
    }
}