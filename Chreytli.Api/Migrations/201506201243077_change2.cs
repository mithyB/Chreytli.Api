namespace Chreytli.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class change2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Favorites", "Submission_Id", "dbo.Submissions");
            DropIndex("dbo.Favorites", new[] { "Submission_Id" });
            RenameColumn(table: "dbo.Favorites", name: "Submission_Id", newName: "SubmissionId");
            AlterColumn("dbo.Favorites", "SubmissionId", c => c.Int(nullable: false));
            CreateIndex("dbo.Favorites", "SubmissionId");
            AddForeignKey("dbo.Favorites", "SubmissionId", "dbo.Submissions", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Favorites", "SubmissionId", "dbo.Submissions");
            DropIndex("dbo.Favorites", new[] { "SubmissionId" });
            AlterColumn("dbo.Favorites", "SubmissionId", c => c.Int());
            RenameColumn(table: "dbo.Favorites", name: "SubmissionId", newName: "Submission_Id");
            CreateIndex("dbo.Favorites", "Submission_Id");
            AddForeignKey("dbo.Favorites", "Submission_Id", "dbo.Submissions", "Id");
        }
    }
}
