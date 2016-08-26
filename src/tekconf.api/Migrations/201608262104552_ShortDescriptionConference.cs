namespace TekConf.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ShortDescriptionConference : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ConferenceInstances", "ShortDescription", c => c.String(nullable: false, maxLength: 500));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ConferenceInstances", "ShortDescription");
        }
    }
}
