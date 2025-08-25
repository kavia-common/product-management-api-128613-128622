using System.ComponentModel.DataAnnotations;

namespace ApiServer.DTOs
{
    /// <summary>
    /// DTO used when creating a new product.
    /// </summary>
    public class CreateProductDto
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }
    }

    /// <summary>
    /// DTO used when updating an existing product.
    /// </summary>
    public class UpdateProductDto
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }
    }

    /// <summary>
    /// DTO returned by the API when representing a product.
    /// </summary>
    public class ProductResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
