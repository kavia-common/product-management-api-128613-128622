using ApiServer.Data;
using ApiServer.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiServer.Repositories
{
    /// <inheritdoc />
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _db;

        public ProductRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Product>> GetAllAsync(CancellationToken ct = default)
        {
            return await _db.Products.AsNoTracking().ToListAsync(ct);
        }

        public async Task<Product?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await _db.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id, ct);
        }

        public async Task<Product> CreateAsync(Product product, CancellationToken ct = default)
        {
            _db.Products.Add(product);
            await _db.SaveChangesAsync(ct);
            return product;
        }

        public async Task<bool> UpdateAsync(int id, Product product, CancellationToken ct = default)
        {
            var existing = await _db.Products.FirstOrDefaultAsync(p => p.Id == id, ct);
            if (existing == null) return false;

            existing.Name = product.Name;
            existing.Price = product.Price;

            await _db.SaveChangesAsync(ct);
            return true;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
        {
            var existing = await _db.Products.FirstOrDefaultAsync(p => p.Id == id, ct);
            if (existing == null) return false;

            _db.Products.Remove(existing);
            await _db.SaveChangesAsync(ct);
            return true;
        }
    }
}
