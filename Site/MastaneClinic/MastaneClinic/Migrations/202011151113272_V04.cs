namespace Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V04 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Experts", "Email", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Experts", "Email");
        }
    }
}
