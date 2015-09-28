namespace Chreytli.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class entities : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "AuthorId", c => c.String());
            AddColumn("dbo.Events", "Date", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Events", "Date");
            DropColumn("dbo.Events", "AuthorId");
        }
    }
}
