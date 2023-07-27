namespace ECommerceCheckout.Domain.Models
{
    /// <summary>
    /// Watch item in the catalog.
    /// </summary>
    public class Watch
    {
        /// <summary>
        /// Gets or sets the ID of the watch.
        /// </summary>
        public string WatchId { get; set; }

        /// <summary>
        /// Gets or sets the name of the watch.
        /// </summary>
        public string WatchName { get; set; }

        /// <summary>
        /// Gets or sets the unit price of the watch.
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// Gets or sets the quantity at which a discount is applicable.
        /// If the customer buys at least this quantity, a discount will be applied.
        /// </summary>
        public int DiscountQuantity { get; set; }

        /// <summary>
        /// Gets or sets the discounted price for a quantity equal to or greater than the <see cref="DiscountQuantity"/>.
        /// </summary>
        public decimal DiscountPrice { get; set; }
    }
}
