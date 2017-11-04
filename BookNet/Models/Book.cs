using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BookNet.Models
{
    public enum Genre
    {
        Action,
        Horror,
        Mystery,
        Romance,
        Drama,
        Satire,
        Comedy,
        ScienceFiction,
        Adventure,
        Poetry,
        History        
    }

    public class Book
    {
        #region Properties

        [Key]
        [Required]
        public int ID { get; set; }

        [Required]
        [MaxLength(30)]
        public string Title { get; set; }

        [Required]
        [MaxLength(200)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required]
        [EnumDataType(typeof(Genre))]
        public Genre Genre { get; set; }

        [Required]        
        [Display(Name = "Price(USD)")]
        [Range(0, int.MaxValue)]
        public decimal Price { get; set; }

        [DataType(DataType.Upload)]
        public string Image { get; set; }

        [ForeignKey("Author")]
        [Display(Name = "Author")]
        public int AuthorID { get; set; }

        #endregion

        #region Ctor

        public Book()
        {
            this.Customers = new HashSet<Customer>();
        }

        #endregion

        #region Navigate Properties

        public Author Author { get; set; }

        public ICollection<Customer> Customers { get; set; }

        #endregion
    }
}