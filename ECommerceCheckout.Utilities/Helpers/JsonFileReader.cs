using System.IO;
using Newtonsoft.Json;

namespace ECommerceCheckout.Utilities
{
    /// <summary>
    /// A utility class to read JSON data from a file.
    /// </summary>
    public static class JsonFileReader
    {
        /// <summary>
        /// Reads JSON data from the specified file and deserializes it to the specified type.
        /// </summary>
        /// <typeparam name="T">The type to deserialize the JSON data into.</typeparam>
        /// <param name="filePath">The path to the JSON file.</param>
        /// <returns>The deserialized object of type T.</returns>
        public static T ReadJsonFile<T>(string filePath)
        {
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
