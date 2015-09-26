namespace Chreytli.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class submissions : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Submissions", "IsHosted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Submissions", "IsHosted");
        }
    }
}
