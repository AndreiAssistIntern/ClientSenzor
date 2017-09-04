namespace ClientSenzori.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SEVEN : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Resetings", "SenzorsId", c => c.Int(nullable: false));
            CreateIndex("dbo.Resetings", "SenzorsId");
            AddForeignKey("dbo.Resetings", "SenzorsId", "dbo.Senzors", "id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Resetings", "SenzorsId", "dbo.Senzors");
            DropIndex("dbo.Resetings", new[] { "SenzorsId" });
            DropColumn("dbo.Resetings", "SenzorsId");
        }
    }
}
