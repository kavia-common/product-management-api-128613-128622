using ApiServer.Models;

namespace ApiServer.Repositories
{
    // PUBLIC_INTERFACE
    /// <summary>
    /// Abstraction over product data access operations.
    /// </summary>
    public interface IProductRepository
    {
        /// <summary>Gets all products.</summary>
        Task<IEnumerable<Product>> GetAllAsync(CancellationToken ct = default);

        /// <summary>Gets a product by id.</summary>
        Task<Product?> GetByIdAsync(int id, CancellationToken ct = default);

        /// <summary>Creates a new product.</summary>
        Task<Product> CreateAsync(Product product, CancellationToken ct = default);

        /// <summary>Updates an existing product.</summary>
        Task<bool> UpdateAsync(int id, Product product, CancellationToken ct = default);

        /// <summary>Deletes a product by id.</summary>
        Task<bool> DeleteAsync(int id, CancellationToken ct = default);
    }
}
