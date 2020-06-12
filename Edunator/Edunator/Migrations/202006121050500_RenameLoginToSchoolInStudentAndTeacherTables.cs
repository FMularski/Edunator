namespace Edunator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameLoginToSchoolInStudentAndTeacherTables : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Students", "School", c => c.String());
            AddColumn("dbo.Teachers", "School", c => c.String());
            DropColumn("dbo.Students", "Login");
            DropColumn("dbo.Teachers", "Login");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Teachers", "Login", c => c.String());
            AddColumn("dbo.Students", "Login", c => c.String());
            DropColumn("dbo.Teachers", "School");
            DropColumn("dbo.Students", "School");
        }
    }
}
