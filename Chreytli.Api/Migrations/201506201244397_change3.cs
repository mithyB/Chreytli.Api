namespace Chreytli.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class change3 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Favorites");
            AddPrimaryKey("dbo.Favorites", new[] { "UserId", "SubmissionId" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.Favorites");
            AddPrimaryKey("dbo.Favorites", "UserId");
        }
    }
}
