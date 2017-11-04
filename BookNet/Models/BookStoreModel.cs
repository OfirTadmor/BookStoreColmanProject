namespace BookNet.Models
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using System.Linq;

    public class BookStoreModel : DbContext
    {
        // Your context has been configured to use a 'BookStoreModel' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'BookNet.Models.BookStoreModel' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'BookStoreModel' 
        // connection string in the application configuration file.

        
    public BookStoreModel()
        : base("name=BookStoreModel")
    {
    }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
         //   Database.SetInitializer<BookStoreModel>(null);

            
            modelBuilder.Entity<Customer>()
                   .Property(p => p.ID)
                   .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            

            base.OnModelCreating(modelBuilder);
    }
    
        public virtual DbSet<Book> Books { get; set; }

        public virtual DbSet<Author> Authors { get; set; }

        public virtual DbSet<Customer> Customers { get; set; }
    }    
}