using System;
using System.Collections.Generic;
using System.Linq;
using ECommerceCheckout.Domain.Models;
using ECommerceCheckout.Utilities.Exceptions;

namespace ECommerceCheckout.Domain.Services
{
    /// <summary>
    /// Represents a service to calculate the total cost for a list of watches.
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
            if (watchIds == null)
            {
                throw new ArgumentNullException(nameof(watchIds), "Watch IDs list cannot be null.");
            }

            if (watchIds.Count == 0)
            {
                return 0;
            }

            Dictionary<string, int> watchQuantities = new Dictionary<string, int>();

            foreach (string watchId in watchIds)
            {
                if (watchQuantities.ContainsKey(watchId))
                {
                    watchQuantities[watchId]++;
                }
                else
                {
                    watchQuantities[watchId] = 1;
                }
            }

            decimal totalCost = 0;

            foreach (var kvp in watchQuantities)
            {
                Watch watch = _watches.FirstOrDefault(w => w.WatchId == kvp.Key);

                if (watch == null)
                {
                    throw new WatchNotFoundException($"Watch with ID '{kvp.Key}' not found in the catalog.");
                }

                totalCost += CalculateWatchCost(watch, kvp.Value);
            }

            return totalCost;
        }

        private decimal CalculateWatchCost(Watch watch, int quantity)
        {
            if (watch.DiscountQuantity > 0 && watch.DiscountPrice > 0)
            {
                int discountedSets = quantity / watch.DiscountQuantity;
                int remainingWatches = quantity % watch.DiscountQuantity;

                return (discountedSets * watch.DiscountPrice) + (remainingWatches * watch.UnitPrice);
            }
            else
            {
                return watch.UnitPrice * quantity;
            }
        }
    }
}
