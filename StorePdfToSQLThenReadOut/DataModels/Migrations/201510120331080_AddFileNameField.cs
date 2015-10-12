namespace DataModels.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFileNameField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SignedContracts", "FileName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SignedContracts", "FileName");
        }
    }
}
