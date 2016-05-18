using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClothesShop.Domain.Entities;

namespace ClothesShop.WebUI.Models
{
    public class ItemViewModel
    {
        public Product Item { get; set; }
        public List<Review> Reviews { get; set; }
        public int ReviewsCount { get; set; }
    }
}