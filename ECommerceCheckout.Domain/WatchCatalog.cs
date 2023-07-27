using System.Collections.Generic;
using System.IO;
using System.Linq;
using ECommerceCheckout.Domain.Models;
using ECommerceCheckout.Utilities;

namespace ECommerceCheckout.Domain
{
    /// <summary>
    /// Represents a catalog of watches with their associated prices.
    /// </summary>
    public static class WatchCatalog
    {
        /// <summary>
        /// Gets the list of watches in the catalog.
        /// </summary>
        public static readonly List<Watch> Watches;

        static WatchCatalog()
        {
            string baseDirectory = Directory.GetCurrentDirectory();
            string domainProjectDirectory = Path.GetFullPath(Path.Combine(baseDirectory, "..", "ECommerceCheckout.Domain"));
            string jsonFilePath = Path.Combine(domainProjectDirectory, "Data", "watch_catalog.json");

            Watches = JsonFileReader.ReadJsonFile<List<Watch>>(jsonFilePath);
        }

        /// <summary>
        /// Gets a watch from the catalog by its ID.
        /// </summary>
        /// <param name="watchId">The ID of the watch to retrieve.</param>
        /// <returns>The watch with the specified ID, or null if not found.</returns>
        public static Watch GetWatchById(string watchId)
        {
            return Watches.FirstOrDefault(watch => watch.WatchId == watchId);
        }
    }
}
