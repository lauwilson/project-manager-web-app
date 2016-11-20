namespace Hemlock.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Employee",
                c => new
                    {
                        EmployeeID = c.Guid(nullable: false),
                        Email = c.String(),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        positionID = c.Guid(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(),
                        Permissions = c.Int(),
                        LastNotified = c.DateTime(),
                    })
                .PrimaryKey(t => t.EmployeeID)
                .ForeignKey("dbo.Position", t => t.positionID)
                .Index(t => t.positionID);
            
            CreateTable(
                "dbo.Position",
                c => new
                    {
                        PositionID = c.Guid(nullable: false),
                        PositionName = c.String(),
                    })
                .PrimaryKey(t => t.PositionID);
            
            CreateTable(
                "dbo.ProjectEntry",
                c => new
                    {
                        ProjectEntryID = c.Guid(nullable: false),
                        CreatedBy = c.Guid(nullable: false),
                        DateCreated = c.DateTime(),
                        ProjectID = c.Guid(nullable: false),
                        ChangeListNo = c.String(),
                        SREDCategoryID = c.Guid(nullable: false),
                        Hours = c.Int(nullable: false),
                        Description = c.String(),
                        ModifiedBy = c.Guid(nullable: false),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ProjectEntryID)
                .ForeignKey("dbo.Employee", t => t.CreatedBy)
                .ForeignKey("dbo.Project", t => t.ProjectID)
                .ForeignKey("dbo.SREDCategory", t => t.SREDCategoryID)
                .Index(t => t.CreatedBy)
                .Index(t => t.ProjectID)
                .Index(t => t.SREDCategoryID);
            
            CreateTable(
                "dbo.Project",
                c => new
                    {
                        ProjectID = c.Guid(nullable: false),
                        ProjectManagerID = c.Guid(nullable: false),
                        ProjectName = c.String(),
                        PicturePath = c.String(),
                        ProjectCreatorID = c.Guid(nullable: false),
                        CreatedDate = c.DateTime(),
                        LastModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ProjectID);
            
            CreateTable(
                "dbo.SREDCategory",
                c => new
                    {
                        SREDCategoryID = c.Guid(nullable: false),
                        ProjectID = c.Guid(nullable: false),
                        CategoryName = c.String(),
                    })
                .PrimaryKey(t => t.SREDCategoryID)
                .ForeignKey("dbo.Project", t => t.ProjectID)
                .Index(t => t.ProjectID);
            
            CreateTable(
                "dbo.Permission",
                c => new
                    {
                        Bit = c.Int(nullable: false, identity: true),
                        PermissionType = c.String(),
                    })
                .PrimaryKey(t => t.Bit);
            
            CreateTable(
                "dbo.TransactionLog",
                c => new
                    {
                        TransactionID = c.Guid(nullable: false),
                        ChangeDescription = c.String(),
                        ChangeDate = c.DateTime(nullable: false),
                        ChangedBy = c.Guid(nullable: false),
                        ErrorMessage = c.String(),
                    })
                .PrimaryKey(t => t.TransactionID)
                .ForeignKey("dbo.Employee", t => t.ChangedBy)
                .Index(t => t.ChangedBy);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TransactionLog", "ChangedBy", "dbo.Employee");
            DropForeignKey("dbo.ProjectEntry", "SREDCategoryID", "dbo.SREDCategory");
            DropForeignKey("dbo.SREDCategory", "ProjectID", "dbo.Project");
            DropForeignKey("dbo.ProjectEntry", "ProjectID", "dbo.Project");
            DropForeignKey("dbo.ProjectEntry", "CreatedBy", "dbo.Employee");
            DropForeignKey("dbo.Employee", "positionID", "dbo.Position");
            DropIndex("dbo.TransactionLog", new[] { "ChangedBy" });
            DropIndex("dbo.SREDCategory", new[] { "ProjectID" });
            DropIndex("dbo.ProjectEntry", new[] { "SREDCategoryID" });
            DropIndex("dbo.ProjectEntry", new[] { "ProjectID" });
            DropIndex("dbo.ProjectEntry", new[] { "CreatedBy" });
            DropIndex("dbo.Employee", new[] { "positionID" });
            DropTable("dbo.TransactionLog");
            DropTable("dbo.Permission");
            DropTable("dbo.SREDCategory");
            DropTable("dbo.Project");
            DropTable("dbo.ProjectEntry");
            DropTable("dbo.Position");
            DropTable("dbo.Employee");
        }
    }
}
