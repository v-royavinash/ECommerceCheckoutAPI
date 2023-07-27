using System;
using System.Collections.Generic;

namespace ECommerceCheckout.Domain.Services
{
    /// <summary>
    /// Represents the service responsible for calculating the total cost of watches during checkout.
    /// </summary>
    public interface ICheckoutService
    {
        /// <summary>
        /// Calculates the total cost for a list of watches during checkout.
        /// </summary>
        /// <param name="watchIds">The list of watch IDs to calculate the cost.</param>
        /// <returns>The total cost of the watches.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="watchIds"/> is null.</exception>
        decimal CalculateTotalCost(List<string> watchIds);
    }
}
