namespace ClientSenzori.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Second : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clients", "PathImage", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Clients", "PathImage");
        }
    }
}
