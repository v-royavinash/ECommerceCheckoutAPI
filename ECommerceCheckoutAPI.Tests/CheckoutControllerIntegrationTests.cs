using System.Collections.Generic;
using ECommerceCheckout.Domain.Models;
using ECommerceCheckout.Domain.Services;
using ECommerceCheckout.Utilities.Exceptions;
using ECommerceCheckoutAPI.Controllers;
using ECommerceCheckoutAPI.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace ECommerceCheckoutAPI.Tests
{
    [TestFixture]
    public class CheckoutControllerIntegrationTests
    {
        [Test]
        public void Post_ValidWatchIds_ReturnsTotalCost()
        {
            var watchCatalog = new List<Watch>
            {
                new Watch { WatchId = "001", UnitPrice = 100, DiscountQuantity = 3, DiscountPrice = 200 }
            };

            var checkoutServiceMock = new Mock<ICheckoutService>();
            checkoutServiceMock.Setup(service => service.CalculateTotalCost(It.IsAny<List<string>>()))
                               .Returns<List<string>>((watchIds) =>
                               {
                                   if (watchIds.Contains("001"))
                                       return 200;
                                   return 0;
                               });

            var loggerMock = new Mock<ILogger<CheckoutController>>();
            var controller = new CheckoutController(checkoutServiceMock.Object, loggerMock.Object);
            var watchIds = new List<string> { "001", "001", "001" };
            var result = controller.Post(watchIds);
            var expectedResult = new WatchCatalogItem { TotalCost = 200 };
            result.Result.Should().BeOfType<OkObjectResult>().Subject.Value.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void Post_EmptyWatchIds_ReturnsBadRequest()
        {
            var checkoutServiceMock = new Mock<ICheckoutService>();
            checkoutServiceMock.Setup(service => service.CalculateTotalCost(It.IsAny<List<string>>()))
                               .Returns(0);

            var loggerMock = new Mock<ILogger<CheckoutController>>();
            var controller = new CheckoutController(checkoutServiceMock.Object, loggerMock.Object);
            var watchIds = new List<string>();
            var result = controller.Post(watchIds);
            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Test]
        public void Post_NullWatchIds_ReturnsBadRequest()
        {
            var checkoutServiceMock = new Mock<ICheckoutService>();
            var loggerMock = new Mock<ILogger<CheckoutController>>();
            var controller = new CheckoutController(checkoutServiceMock.Object, loggerMock.Object);
            var result = controller.Post(null);
            result.Result.Should().BeOfType<BadRequestObjectResult>().Subject.Value.Should().Be("No watch IDs provided.");
        }

        [Test]
        public void Post_WatchNotFound_ReturnsNotFound()
        {
            var watchCatalog = new List<Watch>
            {
                new Watch { WatchId = "001", UnitPrice = 100, DiscountQuantity = 3, DiscountPrice = 200 }
            };

            var checkoutServiceMock = new Mock<ICheckoutService>();
            checkoutServiceMock.Setup(service => service.CalculateTotalCost(It.IsAny<List<string>>()))
                               .Throws(new WatchNotFoundException("Watch with ID '002' not found in the catalog."));
            var loggerMock = new Mock<ILogger<CheckoutController>>();
            var controller = new CheckoutController(checkoutServiceMock.Object, loggerMock.Object);
            var watchIds = new List<string> { "001", "002" };
            var result = controller.Post(watchIds);
            result.Result.Should().BeOfType<NotFoundObjectResult>().Subject.Value.Should().Be("Watch with ID '002' not found in the catalog.");
        }
    }
}
