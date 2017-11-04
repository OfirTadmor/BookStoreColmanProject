using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookNet.Models
{
    public class Author
    {
        #region Properties

        [Key]
        [Required]
        public int ID { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]        
        [Range(1, 120)]
        public int Age { get; set; }

        [DataType(DataType.Upload)]
        public string Image { get; set; }

        [Required]
        [EnumDataType(typeof(Genre))]
        public Genre Specialty { get; set; }

        #endregion

        #region Navigate Properties

        public ICollection<Book> Books { get; set; }

        #endregion
    }
}