namespace Gamer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Game : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Game", "Plataforma", c => c.String(unicode: false));
            AddColumn("dbo.Game", "Preco", c => c.Double(nullable: false));
            AddColumn("dbo.Game", "TipoNegocio", c => c.Int(nullable: false));
            AddColumn("dbo.Game", "Descricao", c => c.String(unicode: false));
            AddColumn("dbo.Game", "Img", c => c.String(unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Game", "Img");
            DropColumn("dbo.Game", "Descricao");
            DropColumn("dbo.Game", "TipoNegocio");
            DropColumn("dbo.Game", "Preco");
            DropColumn("dbo.Game", "Plataforma");
        }
    }
}
