namespace BookNet.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Models.BookStoreModel>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Models.BookStoreModel context)
        {
            context.Database.Initialize(true);
            context.Authors.RemoveRange(context.Authors.ToList());
            context.Books.RemoveRange(context.Books.ToList());
            context.Customers.RemoveRange(context.Customers.ToList());
            context.SaveChanges();

            var author1 = context.Authors.Add(
                new Models.Author
                {
                    FirstName = "DONNA",
                    LastName = "LEON",
                    Age = 74,
                    Image = "Donna_Leon.jpg",
                    Specialty = Models.Genre.ScienceFiction
                });
            var author2 = context.Authors.Add(
                new Models.Author
                {
                    FirstName = "NICHOLAS",
                    LastName = "SPARKS",
                    Age = 50,
                    Image = "Sparks_Nicholas.jpg",
                    Specialty = Models.Genre.ScienceFiction
                });
            var author3 = context.Authors.Add(
                new Models.Author
                {
                    FirstName = "JEFFREY",
                    LastName = "ARCHER",
                    Age = 76,
                    Image = "Jeffrey_Archer.jpg",
                    Specialty = Models.Genre.History
                });
            var author4 = context.Authors.Add(
                new Models.Author
                {
                    FirstName = "VICTORIA",
                    LastName = "HISLOP",
                    Age = 57,
                    Image = "HISLOP_VICTORIA.jpg",
                    Specialty = Models.Genre.Comedy
                });

            context.SaveChanges();

            var book1 = context.Books.Add(
                new Models.Book
                {
                    AuthorID = author3.ID,
                    Title = "BEASTLY THINGS",
                    Description = "When a body is found floating in a canal, strangely disfigured and with multiple stab wounds, Commissario Brunetti is called to investigate and is convinced he recognises the man from somewhere.",
                    Genre = Models.Genre.Action,
                    Image = "BEASTLY_THINGS.jpg",
                    Price = 62
                });
            var book2 = context.Books.Add(
                 new Models.Book
                 {
                     AuthorID = author1.ID,
                     Title = "GOLDEN EGG",
                     Description = "Unkonwn",
                     Genre = Models.Genre.ScienceFiction,
                     Image = "unknown.jpg",
                     Price = 108
                 });
            var book3 = context.Books.Add(
                  new Models.Book
                  {
                      AuthorID = author1.ID,
                      Title = "JEWELS OF PARADISE",
                      Description = "From the bestselling author of the Brunetti crime series comes The Jewels of Paradise, a gripping tale of intrigue, music, history and greed and Donna Leon's first stand-alone novel",
                      Genre = Models.Genre.Comedy,
                      Image = "JEWELS_OF_PARADISE.jpg",
                      Price = 55
                  });
            var book4 = context.Books.Add(
                   new Models.Book
                   {
                       AuthorID = author2.ID,
                       Title = "SINS OF THE FATHER",
                       Description = "The master storyteller continues the Clifton saga with this, the second volume. New York, 1939. Tom Bradshaw is arrested for first degree murder. He stands accused of killing his brother.",
                       Genre = Models.Genre.ScienceFiction,
                       Image = "Sins_of_father.jpg",
                       Price = 55
                   });

            book1.Customers.Add(new Models.Customer
            {
                ID = "201335456",
                FirstName = "chen",
                LastName = "goren",
                BirthDate = new DateTime(1994, 11, 15),
                City = "qiryat ono",
                CreationDate = DateTime.Now,
                Email = "chen@goren.com",
                PhoneNumber = "052-5145452",
                Street = "havazelet 8"
            });

            book2.Customers.Add(
               new Models.Customer
               {
                   ID = "205788951",
                   FirstName = "bar",
                   LastName = "mey",
                   BirthDate = new DateTime(1985, 12, 15),
                   City = "qiryat ono",
                   CreationDate = DateTime.Now,
                   Email = "bar@mey.com",
                   PhoneNumber = "054-5145616",
                   Street = "shlomo hamelech 25"
               });

            book3.Customers.Add(
                new Models.Customer
                {
                    ID = "205787013",
                    FirstName = "Dor",
                    LastName = "Dubnov",
                    BirthDate = new DateTime(1994, 7, 17),
                    City = "Tel Aviv",
                    CreationDate = DateTime.Now,
                    Email = "dor@dubnov.com",
                    PhoneNumber = "054-6655266",
                    Street = "Dubnov 8"
                });

            context.SaveChanges();
        }
    }
}
