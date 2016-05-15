using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClothesShop.Domain.Entities;

namespace ClothesShop.Domain.Abstract
{
    public interface IShopRepository
    {
        IEnumerable<Product> Products { get; }
        void SaveProduct(Product product);
        Product DeleteProduct(int productId);

        IEnumerable<User> Users { get; }
        void SaveUser(User user);

        IEnumerable<Review> Reviews { get; }
        void SaveReview(Review review);

        IEnumerable<Purchase> Purchases { get; }
        void SavePurchase(Purchase purchase);
    }
}
