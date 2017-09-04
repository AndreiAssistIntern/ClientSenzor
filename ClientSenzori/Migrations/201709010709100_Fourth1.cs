namespace ClientSenzori.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Fourth1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Resetings", "ClientId", c => c.Int(nullable: false));
            AddColumn("dbo.Resetings", "HasChecked", c => c.Boolean(nullable: false));
            CreateIndex("dbo.Resetings", "ClientId");
            AddForeignKey("dbo.Resetings", "ClientId", "dbo.Clients", "id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Resetings", "ClientId", "dbo.Clients");
            DropIndex("dbo.Resetings", new[] { "ClientId" });
            DropColumn("dbo.Resetings", "HasChecked");
            DropColumn("dbo.Resetings", "ClientId");
        }
    }
}
