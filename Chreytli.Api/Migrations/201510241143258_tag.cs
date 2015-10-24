namespace Chreytli.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tag : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Submissions", "Tag", c => c.Int(nullable: false, defaultValue: 0));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Submissions", "Tag");
        }
    }
}
