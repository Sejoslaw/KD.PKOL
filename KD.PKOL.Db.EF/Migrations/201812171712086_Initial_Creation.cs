namespace KD.PKOL.Db.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial_Creation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MessageDtoes",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Address = c.String(),
                        Message = c.String(),
                        SendTime = c.DateTime(nullable: false),
                        ErrorTag = c.String(),
                        ErrorMessage = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.MessageDtoes");
        }
    }
}
