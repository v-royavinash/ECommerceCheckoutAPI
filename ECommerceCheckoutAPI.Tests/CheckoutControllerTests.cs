using System;
using System.Collections.Generic;
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
    public class CheckoutControllerTests
    {
        private CheckoutController _controller;
        private Mock<ICheckoutService> _checkoutServiceMock;
        private Mock<ILogger<CheckoutController>> _loggerMock;

        [SetUp]
        public void Setup()
        {
            _checkoutServiceMock = new Mock<ICheckoutService>();
            _loggerMock = new Mock<ILogger<CheckoutController>>();
            _controller = new CheckoutController(_checkoutServiceMock.Object, _loggerMock.Object);
        }

        [Test]

        public void Post_WithValidWatchIds_ReturnsTotalCost()
        {
            List<string> watchIds = new List<string> { "watch1", "watch2", "watch3" };
            decimal expectedTotalCost = 150;
            _checkoutServiceMock.Setup(x => x.CalculateTotalCost(watchIds)).Returns(expectedTotalCost);
            ActionResult<WatchCatalogItem> result = _controller.Post(watchIds);
            result.Result.Should().NotBeNull().And.BeOfType<OkObjectResult>();
            result.Result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeOfType<WatchCatalogItem>()
                .Which.TotalCost.Should().Be(expectedTotalCost);
        }

        [Test]
        public void Post_WithNullWatchIds_ReturnsBadRequest()
        {
            List<string> watchIds = null;
            ActionResult<WatchCatalogItem> result = _controller.Post(watchIds);
            result.Result.Should().NotBeNull().And.BeOfType<BadRequestObjectResult>();
            BadRequestObjectResult badRequestResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Subject;
            badRequestResult.StatusCode.Should().Be(400);
            badRequestResult.Value.Should().Be("No watch IDs provided.");
        }

        [Test]
        public void Post_WithEmptyWatchIds_ReturnsBadRequest()
        {
            List<string> watchIds = new List<string>();
            ActionResult<WatchCatalogItem> result = _controller.Post(watchIds);
            result.Result.Should().NotBeNull().And.BeOfType<BadRequestObjectResult>();
            BadRequestObjectResult badRequestResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Subject;
            badRequestResult.StatusCode.Should().Be(400);
            badRequestResult.Value.Should().Be("No watch IDs provided.");
        }

        [Test]
        public void Post_WithWatchNotFoundException_ReturnsNotFound()
        {
            List<string> watchIds = new List<string> { "watch1", "watch2", "watch3" };
            string errorMessage = "Watch not found in the catalog.";
            _checkoutServiceMock.Setup(x => x.CalculateTotalCost(watchIds)).Throws(new WatchNotFoundException(errorMessage));
            ActionResult<WatchCatalogItem> result = _controller.Post(watchIds);
            result.Result.Should().NotBeNull().And.BeOfType<NotFoundObjectResult>();
            NotFoundObjectResult notFoundResult = result.Result.Should().BeOfType<NotFoundObjectResult>().Subject;
            notFoundResult.StatusCode.Should().Be(404);
            notFoundResult.Value.Should().Be(errorMessage);
        }

        [Test]
        public void Post_WithUnexpectedError_ReturnsInternalServerError()
        {
            List<string> watchIds = new List<string> { "watch1", "watch2", "watch3" };
            string errorMessage = "An unexpected error occurred during the checkout process.";
            _checkoutServiceMock.Setup(x => x.CalculateTotalCost(watchIds)).Throws(new Exception(errorMessage));
            ActionResult<WatchCatalogItem> result = _controller.Post(watchIds);
            result.Result.Should().NotBeNull().And.BeOfType<ObjectResult>();
            ObjectResult internalServerErrorResult = result.Result.Should().BeOfType<ObjectResult>().Subject;
            internalServerErrorResult.StatusCode.Should().Be(500);
            internalServerErrorResult.Value.Should().Be(errorMessage);
        }
    }
}
