namespace Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V02 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Services", "ThumbImageUrl", c => c.String());
            DropColumn("dbo.Services", "HomeImageUrl");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Services", "HomeImageUrl", c => c.String());
            DropColumn("dbo.Services", "ThumbImageUrl");
        }
    }
}
