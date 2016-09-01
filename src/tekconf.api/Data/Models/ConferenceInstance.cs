using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TekConf.Api.Data.Models
{
    public class ConferenceInstance : IEntity
    {

        public int Id { get; set; }

        [Index("IX_ConferenceInstanceSlug", order: 1, IsUnique = true)]
        [Required]
        [StringLength(200)]
        public string Slug { get; set; }

        [Index("IX_ConferenceInstanceName", order: 1, IsUnique = true)]
        [Required]
        [StringLength(200)]
        public string Name { get; set; }


        [Required]
        [StringLength(8000, MinimumLength = 20)]
        public string Description { get; set; }

        [Required]
        [StringLength(500, MinimumLength = 20)]
        public string ShortDescription { get; set; }

        [StringLength(100)]
        public string Tagline { get; set; }

        [StringLength(255)]
        public string ImageUrl { get; set; }

        #region Social
        //[StringLength(255)]
        //public string LiveStreamUrl { get; set; }
        //[StringLength(255)]
        //public string FacebookUrl { get; set; }
        //[StringLength(255)]
        //public string HomepageUrl { get; set; }
        //[StringLength(255)]
        //public string LanyrdUrl { get; set; }
        //[StringLength(255)]
        //public string MeetupUrl { get; set; }
        //[StringLength(255)]
        //public string GooglePlusUrl { get; set; }
        //[StringLength(255)]
        //public string VimeoUrl { get; set; }
        //[StringLength(255)]
        //public string YoutubeUrl { get; set; }
        //[StringLength(255)]
        //public string GithubUrl { get; set; }
        //[StringLength(255)]
        //public string LinkedInUrl { get; set; }
        //[StringLength(40)]
        //public string TwitterHashTag { get; set; }
        //[StringLength(40)]
        //public string TwitterName { get; set; }
        //[StringLength(255)]
        //public string InstagramUrl { get; set; }
        //[StringLength(255)]
        //public string SnapChatUrl { get; set; }
        #endregion

        #region Address
        [StringLength(100)]
        public string LocationName { get; set; }

        [StringLength(100)]
        public string BuildingName { get; set; }

        public int? StreetNumber { get; set; }

        [StringLength(20)]
        public string StreetNumberSuffix { get; set; }

        [StringLength(100)]
        public string StreetName { get; set; }

        [StringLength(100)]
        public string StreetType { get; set; }

        [StringLength(10)]
        public string StreetDirection { get; set; }

        [StringLength(10)]
        public string AddressType { get; set; }

        [StringLength(10)]
        public string AddressTypeId { get; set; }

        [StringLength(100)]
        public string LocalMunicipality { get; set; }

        [StringLength(100)]
        public string City { get; set; }

        [StringLength(30)]
        public string State { get; set; }

        [StringLength(100)]
        public string GoverningDistrict { get; set; }

        [StringLength(10)]
        public string PostalArea { get; set; }

        [StringLength(150)]
        public string Country { get; set; }

        public double? Longitude { get; set; }

        public double? Latitude { get; set; }
        #endregion

        #region Dates
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public DateTime? CallForSpeakersOpens { get; set; }
        public DateTime? CallForSpeakersCloses { get; set; }
        public DateTime? RegistrationOpens { get; set; }
        public DateTime? RegistrationCloses { get; set; }
        #endregion

        public int? DefaultTalkLength { get; set; }
        public int? NumberOfSessions { get; set; }

        public bool IsOnline { get; set; }
        public bool IsLive { get; set; }

        

        #region Relationships
        public virtual Conference Conference { get; set; }
        public virtual ICollection<Session> Sessions { get; set; } = new List<Session>();
        public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
        public virtual ICollection<Difficulty> Difficulties { get; set; } = new List<Difficulty>();

        #endregion

    }
}