using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ClothesShop.Domain.Abstract;
using ClothesShop.Domain.Entities;
using ClothesShop.WebUI.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ClothesShop.UnitTests.Tests
{
    [TestClass]
    public class ImageTests
    {
        [TestMethod]
        public void Can_Retrieve_Image_Data()
        {
            // Организация - создание объекта Game с данными изображения
            Product game = new Product
            {
                Id = 2,
                Name = "Игра2",
                ImageData = new byte[] { },
                ImageMimeType = "image/png"
            };

            // Организация - создание имитированного хранилища
            Mock<IShopRepository> mock = new Mock<IShopRepository>();
            mock.Setup(m => m.Products).Returns(new List<Product> {
                new Product {Id = 1, Name = "Игра1"},
                game,
                new Product {Id = 3, Name = "Игра3"}
            }.AsQueryable());

            // Организация - создание контроллера
            ProductController controller = new ProductController(mock.Object);

            // Действие - вызов метода действия GetImage()
            ActionResult result = controller.GetImage(2);

            // Утверждение
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(FileResult));
            Assert.AreEqual(game.ImageMimeType, ((FileResult)result).ContentType);
        }

        [TestMethod]
        public void Cannot_Retrieve_Image_Data_For_Invalid_ID()
        {
            // Организация - создание имитированного хранилища
            Mock<IShopRepository> mock = new Mock<IShopRepository>();
            mock.Setup(m => m.Products).Returns(new List<Product> {
                new Product {Id = 1, Name = "Игра1"},
                new Product {Id = 2, Name = "Игра2"}
            }.AsQueryable());

            // Организация - создание контроллера
            ProductController controller = new ProductController(mock.Object);

            // Действие - вызов метода действия GetImage()
            ActionResult result = controller.GetImage(10);

            // Утверждение
            Assert.IsNull(result);
        }
    }
}
