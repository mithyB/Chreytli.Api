namespace Chreytli.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Poll : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Choices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        Votes = c.Int(nullable: false),
                        PollId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Polls", t => t.PollId, cascadeDelete: true)
                .Index(t => t.PollId);
            
            CreateTable(
                "dbo.Polls",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        MultipleChoice = c.Boolean(nullable: false),
                        TotalVotes = c.Int(nullable: false),
                        AuthorId = c.String(),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VoteChoices",
                c => new
                    {
                        VoteId = c.Int(nullable: false),
                        ChoiceId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.VoteId, t.ChoiceId })
                .ForeignKey("dbo.Votes", t => t.VoteId, cascadeDelete: true)
                .Index(t => t.VoteId);
            
            CreateTable(
                "dbo.Votes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        PollId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VoteChoices", "VoteId", "dbo.Votes");
            DropForeignKey("dbo.Choices", "PollId", "dbo.Polls");
            DropIndex("dbo.VoteChoices", new[] { "VoteId" });
            DropIndex("dbo.Choices", new[] { "PollId" });
            DropTable("dbo.Votes");
            DropTable("dbo.VoteChoices");
            DropTable("dbo.Polls");
            DropTable("dbo.Choices");
        }
    }
}
