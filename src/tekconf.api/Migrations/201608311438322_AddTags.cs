namespace TekConf.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTags : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Slug = c.String(),
                        Name = c.String(),
                        ConferenceInstance_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ConferenceInstances", t => t.ConferenceInstance_Id)
                .Index(t => t.ConferenceInstance_Id);
            
            CreateTable(
                "dbo.SessionTags",
                c => new
                    {
                        SessionId = c.Int(nullable: false),
                        TagId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.SessionId, t.TagId })
                .ForeignKey("dbo.Sessions", t => t.SessionId)
                .ForeignKey("dbo.Tags", t => t.TagId)
                .Index(t => t.SessionId)
                .Index(t => t.TagId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SessionTags", "TagId", "dbo.Tags");
            DropForeignKey("dbo.SessionTags", "SessionId", "dbo.Sessions");
            DropForeignKey("dbo.Tags", "ConferenceInstance_Id", "dbo.ConferenceInstances");
            DropIndex("dbo.SessionTags", new[] { "TagId" });
            DropIndex("dbo.SessionTags", new[] { "SessionId" });
            DropIndex("dbo.Tags", new[] { "ConferenceInstance_Id" });
            DropTable("dbo.SessionTags");
            DropTable("dbo.Tags");
        }
    }
}
