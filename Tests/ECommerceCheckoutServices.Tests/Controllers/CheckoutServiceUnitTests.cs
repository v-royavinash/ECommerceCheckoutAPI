using System;
using System.Collections.Generic;
using ECommerceCheckout.Domain.Models;
using ECommerceCheckout.Domain.Services;
using ECommerceCheckout.Utilities.Exceptions;
using NUnit.Framework;
using FluentAssertions;

namespace ECommerceCheckout.Domain.Tests
{
    [TestFixture]
    public class CheckoutServiceUnitTests
    {
        private CheckoutService _checkoutService;
        private IEnumerable<Watch> _mockWatches;

        [SetUp]
        public void Setup()
        {
            _mockWatches = new List<Watch>
            {
                new Watch { WatchId = "watch1", UnitPrice = 100, DiscountQuantity = 3, DiscountPrice = 250 },
                new Watch { WatchId = "watch2", UnitPrice = 50, DiscountQuantity = 2, DiscountPrice = 80 },
                new Watch { WatchId = "watch3", UnitPrice = 30, DiscountQuantity = 4, DiscountPrice = 100 },
            };
            _checkoutService = new CheckoutService(_mockWatches);
        }

        [Test]
        public void CalculateTotalCost_NullWatchIds_ThrowsArgumentNullException()
        {
            List<string> watchIds = null;
            Action act = () => _checkoutService.CalculateTotalCost(watchIds);
            act.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void CalculateTotalCost_EmptyWatchIds_ReturnsZero()
        {
            List<string> watchIds = new List<string>();
            decimal totalCost = _checkoutService.CalculateTotalCost(watchIds);
            totalCost.Should().Be(0);
        }

        [Test]
        public void CalculateTotalCost_ValidWatchIds_CalculatesTotalCost()
        {
            List<string> watchIds = new List<string> { "watch1", "watch2", "watch2", "watch3", "watch3", "watch3" };
            // Expected total cost: 100 + 80 (2 * watch2 discount) + 90 (3 * watch3, no discount) = 270
            decimal totalCost = _checkoutService.CalculateTotalCost(watchIds);
            totalCost.Should().Be(270);
        }

        [Test]
        public void CalculateTotalCost_UnknownWatchId_ThrowsWatchNotFoundException()
        {
            List<string> watchIds = new List<string> { "watch1", "watch4" }; // watch4 is not present in the mock watches
            Action act = () => _checkoutService.CalculateTotalCost(watchIds);
            act.Should().Throw<WatchNotFoundException>();
        }
    }
}
