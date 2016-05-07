using System;
using System.Collections.Generic;
using System.Linq;
using ClothesShop.Domain.Abstract;
using ClothesShop.Domain.Entities;
using ClothesShop.WebUI.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ClothesShop.UnitTests
{
    [TestClass]
    public class AdminTests
    {
        [TestMethod]
        public void Index_Contains_All_Products()
        {
            // Организация - создание имитированного хранилища данных
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new List<Product>
            {
                new Product { Id = 1, Name = "Item1"},
                new Product { Id = 2, Name = "Item2"},
                new Product { Id = 3, Name = "Item3"},
                new Product { Id = 4, Name = "Item4"},
                new Product { Id = 5, Name = "Item5"}
            });

            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object);

            // Действие
            List<Product> result = ((IEnumerable<Product>)controller.Index().
                ViewData.Model).ToList();

            // Утверждение
            Assert.AreEqual(result.Count(), 5);
            Assert.AreEqual("Item1", result[0].Name);
            Assert.AreEqual("Item2", result[1].Name);
            Assert.AreEqual("Item3", result[2].Name);
        }

        [TestMethod]
        public void Can_Edit_Game()
        {
            // Организация - создание имитированного хранилища данных
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new List<Product>
            {
                new Product { Id = 1, Name = "Item1"},
                new Product { Id = 2, Name = "Item2"},
                new Product { Id = 3, Name = "Item3"},
                new Product { Id = 4, Name = "Item4"},
                new Product { Id = 5, Name = "Item5"}
            });

            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object);

            // Действие
            Product item1 = controller.Edit(1).ViewData.Model as Product;
            Product item2 = controller.Edit(2).ViewData.Model as Product;
            Product item3 = controller.Edit(3).ViewData.Model as Product;

            // Assert
            Assert.AreEqual(1, item1.Id);
            Assert.AreEqual(2, item2.Id);
            Assert.AreEqual(3, item3.Id);
        }

        [TestMethod]
        public void Can_Delete_Valid_Games()
        {
            // Организация - создание объекта Game
            Product product = new Product { Id = 2, Name = "Item2" };

            // Организация - создание имитированного хранилища данных
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new List<Product>
            {
                new Product { Id = 1, Name = "Item1"},
                new Product { Id = 2, Name = "Item2"},
                new Product { Id = 3, Name = "Item3"},
                new Product { Id = 4, Name = "Item4"},
                new Product { Id = 5, Name = "Item5"}
            });

            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object);

            // Действие - удаление игры
            controller.Delete(product.Id);

            // Утверждение - проверка того, что метод удаления в хранилище
            // вызывается для корректного объекта Game
            mock.Verify(m => m.DeleteProduct(product.Id));
        }

    }
}
