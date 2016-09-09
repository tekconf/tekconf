using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Azure.Mobile.Server;

namespace TekconfApi.DataObjects
{
    public class Conference : EntityData
    {

         [Required]
        [StringLength(200)]
        public string Slug { get; set; }

        [Required]
        [StringLength(300)]
        public string Name { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        [StringLength(8000, MinimumLength = 20)]
        public string Description { get; set; }

        #region Relationships

        //public virtual ICollection<ConferenceInstance> Instances { get; set; } = new List<ConferenceInstance>();
        //public User Owner { get; set; }

        #endregion
    }

    public class ConferenceInstance : EntityData
    {

        [Required]
        [StringLength(200)]
        public string Slug { get; set; }

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

    public class Difficulty : EntityData
    {
        [Required]
        [StringLength(1000)]
        public string Description { get; set; }
        public int Order { get; set; }

        #region Relationships
        public virtual ConferenceInstance ConferenceInstance { get; set; }
        public virtual ICollection<Session> Sessions { get; set; } = new List<Session>();

        #endregion
    }

    public class Presentation : EntityData
    {

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

    public class Session : EntityData
    {

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
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Building { get; set; }
        public string Room { get; set; }

        //public string Difficulty { get; set; }
        public string TwitterHashTag { get; set; }
        //public string SessionType { get; set; }
        //public bool IsAddedToSchedule { get; set; }
        //public List<string> Links { get; set; }
        //public List<string> Subjects { get; set; }
        //public List<string> Resources { get; set; }
        //public List<string> Prerequisites { get; set; }




        #region Relationships
        public ConferenceInstance ConferenceInstance { get; set; }
        public Difficulty Difficulty { get; set; }
        public Presentation Presentation { get; set; }
        public virtual ICollection<Speaker> Speakers { get; set; } = new List<Speaker>();
        public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();

        #endregion
    }

    public class Speaker : EntityData
    {

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

    public class Tag : EntityData
    {

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

    public class User : EntityData
    {
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