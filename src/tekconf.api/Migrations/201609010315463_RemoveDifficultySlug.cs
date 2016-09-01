namespace TekConf.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveDifficultySlug : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Difficulties", "Slug");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Difficulties", "Slug", c => c.String(nullable: false, maxLength: 200));
        }
    }
}
