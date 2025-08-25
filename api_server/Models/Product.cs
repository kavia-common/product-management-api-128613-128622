using System.ComponentModel.DataAnnotations;

namespace ApiServer.Models
{
    // PUBLIC_INTERFACE
    /// <summary>
    /// Represents a product with a unique identifier, name, and price.
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Primary key identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Product name.
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Product price in the store's currency.
        /// </summary>
        [Range(0, double.MaxValue, ErrorMessage = "Price must be a non-negative value.")]
        public decimal Price { get; set; }
    }
}
