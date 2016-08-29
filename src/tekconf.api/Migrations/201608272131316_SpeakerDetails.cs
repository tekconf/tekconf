namespace TekConf.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SpeakerDetails : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Speakers", "Title", c => c.String(maxLength: 200));
            AddColumn("dbo.Speakers", "Company", c => c.String(maxLength: 200));
            AlterColumn("dbo.Speakers", "BlogUrl", c => c.String(maxLength: 300));
            AlterColumn("dbo.Speakers", "EmailAddress", c => c.String(maxLength: 200));
            AlterColumn("dbo.Speakers", "PhoneNumber", c => c.String(maxLength: 30));
            AlterColumn("dbo.Speakers", "TwitterName", c => c.String(maxLength: 100));
            AlterColumn("dbo.Speakers", "FacebookUrl", c => c.String(maxLength: 300));
            AlterColumn("dbo.Speakers", "LinkedInUrl", c => c.String(maxLength: 300));
            AlterColumn("dbo.Speakers", "ProfileImageUrl", c => c.String(maxLength: 300));
            AlterColumn("dbo.Speakers", "GooglePlusUrl", c => c.String(maxLength: 300));
            AlterColumn("dbo.Speakers", "VimeoUrl", c => c.String(maxLength: 300));
            AlterColumn("dbo.Speakers", "YoutubeUrl", c => c.String(maxLength: 300));
            AlterColumn("dbo.Speakers", "GithubUrl", c => c.String(maxLength: 300));
            AlterColumn("dbo.Speakers", "CoderWallUrl", c => c.String(maxLength: 300));
            AlterColumn("dbo.Speakers", "StackoverflowUrl", c => c.String(maxLength: 300));
            AlterColumn("dbo.Speakers", "BitbucketUrl", c => c.String(maxLength: 300));
            AlterColumn("dbo.Speakers", "CodeplexUrl", c => c.String(maxLength: 300));
            AlterColumn("dbo.Speakers", "InstagramUrl", c => c.String(maxLength: 300));
            AlterColumn("dbo.Speakers", "SnapchatUrl", c => c.String(maxLength: 300));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Speakers", "SnapchatUrl", c => c.String());
            AlterColumn("dbo.Speakers", "InstagramUrl", c => c.String());
            AlterColumn("dbo.Speakers", "CodeplexUrl", c => c.String());
            AlterColumn("dbo.Speakers", "BitbucketUrl", c => c.String());
            AlterColumn("dbo.Speakers", "StackoverflowUrl", c => c.String());
            AlterColumn("dbo.Speakers", "CoderWallUrl", c => c.String());
            AlterColumn("dbo.Speakers", "GithubUrl", c => c.String());
            AlterColumn("dbo.Speakers", "YoutubeUrl", c => c.String());
            AlterColumn("dbo.Speakers", "VimeoUrl", c => c.String());
            AlterColumn("dbo.Speakers", "GooglePlusUrl", c => c.String());
            AlterColumn("dbo.Speakers", "ProfileImageUrl", c => c.String());
            AlterColumn("dbo.Speakers", "LinkedInUrl", c => c.String());
            AlterColumn("dbo.Speakers", "FacebookUrl", c => c.String());
            AlterColumn("dbo.Speakers", "TwitterName", c => c.String());
            AlterColumn("dbo.Speakers", "PhoneNumber", c => c.String());
            AlterColumn("dbo.Speakers", "EmailAddress", c => c.String());
            AlterColumn("dbo.Speakers", "BlogUrl", c => c.String());
            DropColumn("dbo.Speakers", "Company");
            DropColumn("dbo.Speakers", "Title");
        }
    }
}
