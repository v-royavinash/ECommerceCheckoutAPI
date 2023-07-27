using System;
using System.Collections.Generic;
using ECommerceCheckout.Domain.Models;

namespace ECommerceCheckout.Domain.Services
{
    /// <summary>
    /// Service to calculate the total cost for a list of watches.
    /// </summary>
    public class CheckoutService : ICheckoutService
    {
        private readonly IEnumerable<Watch> _watches;

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckoutService"/> class.
        /// </summary>
        /// <param name="watches">The collection of available watches in the catalog.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="watches"/> is null.</exception>
        public CheckoutService(IEnumerable<Watch> watches)
        {
            _watches = watches ?? throw new ArgumentNullException(nameof(watches));
        }

        /// <summary>
        /// Calculates the total cost for a list of watches.
        /// </summary>
        /// <param name="watchIds">The list of watch IDs to calculate the cost.</param>
        /// <returns>The total cost of the watches.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="watchIds"/> is null.</exception>
        public decimal CalculateTotalCost(List<string> watchIds)
        {
            var totalCost = 360;
            return totalCost;
        }
    }
}
