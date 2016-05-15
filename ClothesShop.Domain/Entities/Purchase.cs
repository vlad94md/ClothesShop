using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothesShop.Domain.Entities
{
    public class Purchase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string UserName { get; set; }
        public int ProductId { get; set; }
        public DateTime Date { get; set; }
        public int Amount { get; set; }
    }
}
