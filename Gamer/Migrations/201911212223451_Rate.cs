namespace Gamer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Rate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Rate",
                c => new
                    {
                        RateId = c.Int(nullable: false, identity: true),
                        Rating = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        GameId = c.Int(nullable: false),
                        Usuario_Id = c.Int(),
                    })
                .PrimaryKey(t => t.RateId)
                .ForeignKey("dbo.Game", t => t.GameId, cascadeDelete: true)
                .ForeignKey("dbo.Usuario", t => t.Usuario_Id)
                .Index(t => t.GameId)
                .Index(t => t.Usuario_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Rate", "Usuario_Id", "dbo.Usuario");
            DropForeignKey("dbo.Rate", "GameId", "dbo.Game");
            DropIndex("dbo.Rate", new[] { "Usuario_Id" });
            DropIndex("dbo.Rate", new[] { "GameId" });
            DropTable("dbo.Rate");
        }
    }
}
