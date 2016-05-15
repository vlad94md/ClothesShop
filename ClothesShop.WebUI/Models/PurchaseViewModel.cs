using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClothesShop.WebUI.Models
{
    public class PurchaseViewModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public int ProductId { get; set; }
        public DateTime Date { get; set; }
        public string ProductName { get; set; }
        public int Amount { get; set; }
    }
}