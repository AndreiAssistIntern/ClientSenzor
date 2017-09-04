namespace ClientSenzori.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class six : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Resetings", "ClientId", "dbo.Clients");
            DropIndex("dbo.Resetings", new[] { "ClientId" });
            RenameColumn(table: "dbo.Resetings", name: "ClientId", newName: "Clients_id");
            AlterColumn("dbo.Resetings", "Clients_id", c => c.Int());
            CreateIndex("dbo.Resetings", "Clients_id");
            AddForeignKey("dbo.Resetings", "Clients_id", "dbo.Clients", "id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Resetings", "Clients_id", "dbo.Clients");
            DropIndex("dbo.Resetings", new[] { "Clients_id" });
            AlterColumn("dbo.Resetings", "Clients_id", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.Resetings", name: "Clients_id", newName: "ClientId");
            CreateIndex("dbo.Resetings", "ClientId");
            AddForeignKey("dbo.Resetings", "ClientId", "dbo.Clients", "id", cascadeDelete: true);
        }
    }
}
