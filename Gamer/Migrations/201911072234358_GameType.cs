namespace Gamer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GameType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Game", "Categoria", c => c.String(unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Game", "Categoria");
        }
    }
}
