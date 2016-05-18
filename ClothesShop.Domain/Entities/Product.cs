using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ClothesShop.Domain.Entities
{
    public class Product
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "Please, enter item name")]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Description")]
        [Required(ErrorMessage = "Please, enter item description")]
        public string Description { get; set; }

        [Display(Name = "Category")]
        [Required(ErrorMessage = "Please, enter category for item")]
        public string Category { get; set; }

        [Display(Name = "Price")]
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Please, enter a possitve number for price")]
        public decimal Price { get; set; }

        public byte[] ImageData { get; set; }
        public string ImageMimeType { get; set; }
        public double Rate { get; set; }
        public int ReviewsNumber { get; set; }
    }
}
