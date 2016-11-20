namespace Hemlock.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NullableSREDCategoryID : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.ProjectEntry", new[] { "SREDCategoryID" });
            AlterColumn("dbo.ProjectEntry", "DateCreated", c => c.DateTime(nullable: false));
            AlterColumn("dbo.ProjectEntry", "SREDCategoryID", c => c.Guid());
            CreateIndex("dbo.ProjectEntry", "SREDCategoryID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.ProjectEntry", new[] { "SREDCategoryID" });
            AlterColumn("dbo.ProjectEntry", "SREDCategoryID", c => c.Guid(nullable: false));
            AlterColumn("dbo.ProjectEntry", "DateCreated", c => c.DateTime());
            CreateIndex("dbo.ProjectEntry", "SREDCategoryID");
        }
    }
}
