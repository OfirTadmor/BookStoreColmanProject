namespace BookNet.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Authors",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        Age = c.Int(nullable: false),
                        Image = c.String(),
                        Specialty = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Books",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 30),
                        Description = c.String(nullable: false, maxLength: 200),
                        Genre = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Image = c.String(),
                        AuthorID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Authors", t => t.AuthorID, cascadeDelete: true)
                .Index(t => t.AuthorID);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        City = c.String(nullable: false),
                        Street = c.String(nullable: false),
                        PhoneNumber = c.String(nullable: false),
                        BirthDate = c.DateTime(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.CustomerBooks",
                c => new
                    {
                        Customer_ID = c.String(nullable: false, maxLength: 128),
                        Book_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Customer_ID, t.Book_ID })
                .ForeignKey("dbo.Customers", t => t.Customer_ID, cascadeDelete: true)
                .ForeignKey("dbo.Books", t => t.Book_ID, cascadeDelete: true)
                .Index(t => t.Customer_ID)
                .Index(t => t.Book_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CustomerBooks", "Book_ID", "dbo.Books");
            DropForeignKey("dbo.CustomerBooks", "Customer_ID", "dbo.Customers");
            DropForeignKey("dbo.Books", "AuthorID", "dbo.Authors");
            DropIndex("dbo.CustomerBooks", new[] { "Book_ID" });
            DropIndex("dbo.CustomerBooks", new[] { "Customer_ID" });
            DropIndex("dbo.Books", new[] { "AuthorID" });
            DropTable("dbo.CustomerBooks");
            DropTable("dbo.Customers");
            DropTable("dbo.Books");
            DropTable("dbo.Authors");
        }
    }
}
