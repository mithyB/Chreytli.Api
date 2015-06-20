namespace Chreytli.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Favorites",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        Submission_Id = c.Int(),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.Submissions", t => t.Submission_Id)
                .Index(t => t.Submission_Id);
            
            CreateTable(
                "dbo.Submissions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AuthorId = c.String(),
                        Img = c.String(),
                        Url = c.String(),
                        Date = c.DateTime(nullable: false),
                        Score = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Favorites", "Submission_Id", "dbo.Submissions");
            DropIndex("dbo.Favorites", new[] { "Submission_Id" });
            DropTable("dbo.Submissions");
            DropTable("dbo.Favorites");
        }
    }
}
