using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClothesShop.Domain.Abstract;
using ClothesShop.Domain.Entities;

namespace ClothesShop.Domain.Concrete
{
    public class EFShopRepository : IShopRepository
    {
        EFDbContext context = new EFDbContext();

        public IEnumerable<Product> Products
        {
            get { return context.Products; }
        }

        public IEnumerable<User> Users
        {
            get { return context.Users; }
        }

        public void SaveUser(User user)
        {
            var userWithThatUsername = context.Users.FirstOrDefault(x => x.Username == user.Username);

            if (userWithThatUsername == null)
                context.Users.Add(user);
            else
            {
                User dbEntry = context.Users.Find(user.Username);
                if (dbEntry != null)
                {
                    dbEntry.Name = user.Name;
                    dbEntry.Username = user.Username;
                    dbEntry.Number = user.Number;
                    dbEntry.Email = user.Email;
                    dbEntry.City = user.City;
                    dbEntry.Country = user.Country;
                    dbEntry.Line1 = user.Line1;
                    dbEntry.Line2 = user.Line2;
                    dbEntry.Password = user.Password;


                }
            }

            context.SaveChanges();
        }

        public IEnumerable<Review> Reviews
        {
            get { return context.Reviews; }
        }

        public void SaveReview(Review review)
        {
            context.Reviews.Add(review);
            context.SaveChanges();
        }

        public IEnumerable<Purchase> Purchases
        {
            get { return context.Purchases; }
        }

        public void SavePurchase(Purchase purchase)
        {
            context.Purchases.Add(purchase);
            context.SaveChanges();
        }

        public void SaveProduct(Product product)
        {
            if (product.Id == 0)
                context.Products.Add(product);
            else
            {
                Product dbEntry = context.Products.Find(product.Id);
                if (dbEntry != null)
                {
                    dbEntry.Name = product.Name;
                    dbEntry.Description = product.Description;
                    dbEntry.Price = product.Price;
                    dbEntry.Category = product.Category;
                    dbEntry.ImageData = product.ImageData;
                    dbEntry.ImageMimeType = product.ImageMimeType;
                }
            }
            context.SaveChanges();
        }

        public Product DeleteProduct(int productId)
        {
            Product dbEntry = context.Products.Find(productId);
            if (dbEntry != null)
            {
                context.Products.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }
    }
}
