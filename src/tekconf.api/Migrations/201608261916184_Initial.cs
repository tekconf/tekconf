namespace TekConf.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ConferenceInstances",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Slug = c.String(nullable: false, maxLength: 200),
                        Name = c.String(nullable: false, maxLength: 200),
                        Description = c.String(nullable: false),
                        Tagline = c.String(maxLength: 100),
                        ImageUrl = c.String(maxLength: 255),
                        LocationName = c.String(maxLength: 100),
                        BuildingName = c.String(maxLength: 100),
                        StreetNumber = c.Int(),
                        StreetNumberSuffix = c.String(maxLength: 20),
                        StreetName = c.String(maxLength: 100),
                        StreetType = c.String(maxLength: 100),
                        StreetDirection = c.String(maxLength: 10),
                        AddressType = c.String(maxLength: 10),
                        AddressTypeId = c.String(maxLength: 10),
                        LocalMunicipality = c.String(maxLength: 100),
                        City = c.String(maxLength: 100),
                        State = c.String(maxLength: 30),
                        GoverningDistrict = c.String(maxLength: 100),
                        PostalArea = c.String(maxLength: 10),
                        Country = c.String(maxLength: 150),
                        Longitude = c.Double(),
                        Latitude = c.Double(),
                        Start = c.DateTime(),
                        End = c.DateTime(),
                        CallForSpeakersOpens = c.DateTime(),
                        CallForSpeakersCloses = c.DateTime(),
                        RegistrationOpens = c.DateTime(),
                        RegistrationCloses = c.DateTime(),
                        DefaultTalkLength = c.Int(),
                        NumberOfSessions = c.Int(),
                        IsOnline = c.Boolean(nullable: false),
                        IsLive = c.Boolean(nullable: false),
                        Conference_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Conferences", t => t.Conference_Id)
                .Index(t => t.Slug, unique: true, name: "IX_ConferenceInstanceSlug")
                .Index(t => t.Name, unique: true, name: "IX_ConferenceInstanceName")
                .Index(t => t.Conference_Id);
            
            CreateTable(
                "dbo.Conferences",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Slug = c.String(nullable: false, maxLength: 200),
                        Name = c.String(nullable: false, maxLength: 300),
                        CreatedAt = c.DateTime(nullable: false),
                        Description = c.String(nullable: false),
                        Owner_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Owner_Id)
                .Index(t => t.Slug, unique: true, name: "IX_ConferenceSlug")
                .Index(t => t.Name, unique: true, name: "IX_ConferenceName")
                .Index(t => t.Owner_Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Slug = c.String(nullable: false, maxLength: 200),
                        FirstName = c.String(nullable: false, maxLength: 200),
                        LastName = c.String(nullable: false, maxLength: 200),
                        Bio = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Slug, unique: true, name: "IX_UserSlug");
            
            CreateTable(
                "dbo.Presentations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Slug = c.String(nullable: false, maxLength: 200),
                        Title = c.String(nullable: false, maxLength: 500),
                        Description = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Slug, unique: true, name: "IX_PresentationSlug");
            
            CreateTable(
                "dbo.Sessions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Slug = c.String(nullable: false, maxLength: 200),
                        Title = c.String(nullable: false, maxLength: 500),
                        Description = c.String(nullable: false),
                        ConferenceInstance_Id = c.Int(nullable: false),
                        Presentation_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ConferenceInstances", t => t.ConferenceInstance_Id)
                .ForeignKey("dbo.Presentations", t => t.Presentation_Id)
                .Index(t => t.Slug, unique: true, name: "IX_SessionSlug")
                .Index(t => t.ConferenceInstance_Id)
                .Index(t => t.Presentation_Id);
            
            CreateTable(
                "dbo.Speakers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Slug = c.String(nullable: false, maxLength: 200),
                        FirstName = c.String(nullable: false, maxLength: 200),
                        LastName = c.String(nullable: false, maxLength: 200),
                        Bio = c.String(),
                        BlogUrl = c.String(),
                        EmailAddress = c.String(),
                        PhoneNumber = c.String(),
                        TwitterName = c.String(),
                        FacebookUrl = c.String(),
                        LinkedInUrl = c.String(),
                        ProfileImageUrl = c.String(),
                        GooglePlusUrl = c.String(),
                        VimeoUrl = c.String(),
                        YoutubeUrl = c.String(),
                        GithubUrl = c.String(),
                        CoderWallUrl = c.String(),
                        StackoverflowUrl = c.String(),
                        BitbucketUrl = c.String(),
                        CodeplexUrl = c.String(),
                        InstagramUrl = c.String(),
                        SnapchatUrl = c.String(),
                        User_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.Slug, unique: true, name: "IX_SpeakerSlug")
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.SessionSpeakers",
                c => new
                    {
                        SessionId = c.Int(nullable: false),
                        SpeakerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.SessionId, t.SpeakerId })
                .ForeignKey("dbo.Sessions", t => t.SessionId)
                .ForeignKey("dbo.Speakers", t => t.SpeakerId)
                .Index(t => t.SessionId)
                .Index(t => t.SpeakerId);
            
            CreateTable(
                "dbo.UserPresentations",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        PresentationId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.PresentationId })
                .ForeignKey("dbo.Users", t => t.UserId)
                .ForeignKey("dbo.Presentations", t => t.PresentationId)
                .Index(t => t.UserId)
                .Index(t => t.PresentationId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ConferenceInstances", "Conference_Id", "dbo.Conferences");
            DropForeignKey("dbo.Conferences", "Owner_Id", "dbo.Users");
            DropForeignKey("dbo.UserPresentations", "PresentationId", "dbo.Presentations");
            DropForeignKey("dbo.UserPresentations", "UserId", "dbo.Users");
            DropForeignKey("dbo.SessionSpeakers", "SpeakerId", "dbo.Speakers");
            DropForeignKey("dbo.SessionSpeakers", "SessionId", "dbo.Sessions");
            DropForeignKey("dbo.Speakers", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Sessions", "Presentation_Id", "dbo.Presentations");
            DropForeignKey("dbo.Sessions", "ConferenceInstance_Id", "dbo.ConferenceInstances");
            DropIndex("dbo.UserPresentations", new[] { "PresentationId" });
            DropIndex("dbo.UserPresentations", new[] { "UserId" });
            DropIndex("dbo.SessionSpeakers", new[] { "SpeakerId" });
            DropIndex("dbo.SessionSpeakers", new[] { "SessionId" });
            DropIndex("dbo.Speakers", new[] { "User_Id" });
            DropIndex("dbo.Speakers", "IX_SpeakerSlug");
            DropIndex("dbo.Sessions", new[] { "Presentation_Id" });
            DropIndex("dbo.Sessions", new[] { "ConferenceInstance_Id" });
            DropIndex("dbo.Sessions", "IX_SessionSlug");
            DropIndex("dbo.Presentations", "IX_PresentationSlug");
            DropIndex("dbo.Users", "IX_UserSlug");
            DropIndex("dbo.Conferences", new[] { "Owner_Id" });
            DropIndex("dbo.Conferences", "IX_ConferenceName");
            DropIndex("dbo.Conferences", "IX_ConferenceSlug");
            DropIndex("dbo.ConferenceInstances", new[] { "Conference_Id" });
            DropIndex("dbo.ConferenceInstances", "IX_ConferenceInstanceName");
            DropIndex("dbo.ConferenceInstances", "IX_ConferenceInstanceSlug");
            DropTable("dbo.UserPresentations");
            DropTable("dbo.SessionSpeakers");
            DropTable("dbo.Speakers");
            DropTable("dbo.Sessions");
            DropTable("dbo.Presentations");
            DropTable("dbo.Users");
            DropTable("dbo.Conferences");
            DropTable("dbo.ConferenceInstances");
        }
    }
}
