namespace TekConf.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Difficulties : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Difficulties",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Slug = c.String(nullable: false, maxLength: 200),
                        Description = c.String(nullable: false, maxLength: 1000),
                        Order = c.Int(nullable: false),
                        ConferenceInstance_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ConferenceInstances", t => t.ConferenceInstance_Id)
                .Index(t => t.ConferenceInstance_Id);
            
            AddColumn("dbo.Sessions", "Difficulty_Id", c => c.Int());
            AlterColumn("dbo.Tags", "Slug", c => c.String(nullable: false, maxLength: 200));
            AlterColumn("dbo.Tags", "Name", c => c.String(nullable: false, maxLength: 1000));
            CreateIndex("dbo.Sessions", "Difficulty_Id");
            AddForeignKey("dbo.Sessions", "Difficulty_Id", "dbo.Difficulties", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Sessions", "Difficulty_Id", "dbo.Difficulties");
            DropForeignKey("dbo.Difficulties", "ConferenceInstance_Id", "dbo.ConferenceInstances");
            DropIndex("dbo.Difficulties", new[] { "ConferenceInstance_Id" });
            DropIndex("dbo.Sessions", new[] { "Difficulty_Id" });
            AlterColumn("dbo.Tags", "Name", c => c.String());
            AlterColumn("dbo.Tags", "Slug", c => c.String());
            DropColumn("dbo.Sessions", "Difficulty_Id");
            DropTable("dbo.Difficulties");
        }
    }
}
