namespace TekConf.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SessionDates : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sessions", "Start", c => c.DateTime());
            AddColumn("dbo.Sessions", "End", c => c.DateTime());
            AddColumn("dbo.Sessions", "Building", c => c.String());
            AddColumn("dbo.Sessions", "Room", c => c.String());
            AddColumn("dbo.Sessions", "TwitterHashTag", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Sessions", "TwitterHashTag");
            DropColumn("dbo.Sessions", "Room");
            DropColumn("dbo.Sessions", "Building");
            DropColumn("dbo.Sessions", "End");
            DropColumn("dbo.Sessions", "Start");
        }
    }
}
