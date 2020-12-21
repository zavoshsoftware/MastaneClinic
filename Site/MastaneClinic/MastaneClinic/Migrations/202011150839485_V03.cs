namespace Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V03 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ServiceBlogs",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ServiceId = c.Guid(nullable: false),
                        BlogId = c.Guid(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Blogs", t => t.BlogId, cascadeDelete: true)
                .ForeignKey("dbo.Services", t => t.ServiceId, cascadeDelete: true)
                .Index(t => t.ServiceId)
                .Index(t => t.BlogId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ServiceBlogs", "ServiceId", "dbo.Services");
            DropForeignKey("dbo.ServiceBlogs", "BlogId", "dbo.Blogs");
            DropIndex("dbo.ServiceBlogs", new[] { "BlogId" });
            DropIndex("dbo.ServiceBlogs", new[] { "ServiceId" });
            DropTable("dbo.ServiceBlogs");
        }
    }
}
