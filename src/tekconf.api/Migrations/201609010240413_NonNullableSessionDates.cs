namespace TekConf.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NonNullableSessionDates : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Sessions", "Start", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Sessions", "End", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Sessions", "End", c => c.DateTime());
            AlterColumn("dbo.Sessions", "Start", c => c.DateTime());
        }
    }
}
