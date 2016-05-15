using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothesShop.Domain.Entities
{
    public class User
    {
        [Key]
        [Required(ErrorMessage = "Enter your username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Enter your password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Enter your email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Enter your fullname")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Enter your main address")]
        [Display(Name = "First address")]
        public string Line1 { get; set; }

        [Display(Name = "Second address")]
        public string Line2 { get; set; }

        [Display(Name = "Phone number")]
        public string Number { get; set; }

        [Required(ErrorMessage = "Enter city")]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required(ErrorMessage = "Enter your country")]
        [Display(Name = "Country")]
        public string Country { get; set; }
    }
}
