using System;
using ECommerceCheckout.Utilities.Exceptions;
using FluentAssertions;
using NUnit.Framework;

namespace ECommerceCheckout.Utilities.Tests.Exceptions
{

    [TestFixture]
    public class WatchNotFoundExceptionUnitTests
    {
        [Test]
        public void WatchNotFoundException_DefaultConstructor()
        {
            WatchNotFoundException exception = new WatchNotFoundException();
            exception.Should().NotBeNull();
            exception.Should().BeOfType<WatchNotFoundException>();
            exception.InnerException.Should().BeNull();
        }

        [Test]
        public void WatchNotFoundException_MessageConstructor()
        {
            var errorMessage = "Watch not found in the catalog.";
            WatchNotFoundException exception = new WatchNotFoundException(errorMessage);
            exception.Should().NotBeNull();
            exception.Should().BeOfType<WatchNotFoundException>();
            exception.Message.Should().Be(errorMessage);
            exception.InnerException.Should().BeNull();
        }

        [Test]
        public void WatchNotFoundException_MessageAndInnerExceptionConstructor()
        {
            var errorMessage = "Watch not found in the catalog.";
            Exception innerException = new Exception("Inner exception message.");
            WatchNotFoundException exception = new WatchNotFoundException(errorMessage, innerException);
            exception.Should().BeOfType<WatchNotFoundException>();
            exception.Message.Should().Be(errorMessage);
            exception.InnerException.Should().Be(innerException);
        }
    }
}
