using System;
using System.Collections.Generic;
using ECommerceCheckout.Domain.Services;
using ECommerceCheckout.Domain.Models;
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
        private ICheckoutService _checkoutService;
        private CheckoutController _controller;

        [SetUp]
        public void SetUp()
        {
            _checkoutService = new CheckoutService(new List<Watch>
        {
            new Watch { WatchId = "001", UnitPrice = 100, DiscountQuantity = 3, DiscountPrice = 200 },
            new Watch { WatchId = "002", UnitPrice = 150, DiscountQuantity = 2, DiscountPrice = 250 },
        });

            var loggerMock = new Mock<ILogger<CheckoutController>>();
            _controller = new CheckoutController(_checkoutService, loggerMock.Object);
        } 
        [Test]
        public void Post_ValidWatchIds_ReturnsTotalCost()
        {
            var watchIds = new List<string> { "001", "001", "002", "001" };
            var result = _controller.Post(watchIds);

            result.Result.Should().BeOfType<OkObjectResult>()
                    .Which.Value.Should().BeOfType<WatchCatalogItem>()
                    .Which.TotalCost.Should().Be(350);
        }

        [Test]
        public void Post_WatchNotFound_ReturnsNotFound()
        {
            // Act
            var watchIds = new List<string> { "001", "003" }; // Assuming watch ID "003" doesn't exist in the database.
            var result = _controller.Post(watchIds);

            // Assert
            result.Result.Should().BeOfType<NotFoundObjectResult>().Subject.Value.Should().Be("Watch with ID '003' not found in the catalog.");
        }

        [Test]
        public void Post_EmptyWatchIds_ReturnsBadRequest()
        {
            var result = _controller.Post(new List<string>());
            result.Result.Should().BeOfType<BadRequestObjectResult>().Subject.Value.Should().Be("No watch IDs provided.");
        }

        [Test]
        public void Post_NullWatchIds_ReturnsBadRequest()
        {
            var result = _controller.Post(null);
            result.Result.Should().BeOfType<BadRequestObjectResult>().Subject.Value.Should().Be("No watch IDs provided.");
        }

        [Test]
        public void Post_WatchWithDiscount_ReturnsTotalCostWithDiscountApplied()
        {
            var watchIds = new List<string> { "001", "001", "001", "001", "001", "001", "001" };
            var result = _controller.Post(watchIds);
            result.Result.Should().BeOfType<OkObjectResult>()
                    .Which.Value.Should().BeOfType<WatchCatalogItem>()
                    .Which.TotalCost.Should().Be(500);
        }

    }
}