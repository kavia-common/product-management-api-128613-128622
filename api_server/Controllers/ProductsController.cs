using ApiServer.DTOs;
using ApiServer.Models;
using ApiServer.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ApiServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _repo;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IProductRepository repo, ILogger<ProductsController> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        // PUBLIC_INTERFACE
        /// <summary>
        /// Retrieves the list of all products.
        /// </summary>
        /// <returns>Array of products.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProductResponseDto>), StatusCodes.Status200OK)]
        [Tags("Products")]
        [ActionName("GetAllProducts")]
        public async Task<ActionResult<IEnumerable<ProductResponseDto>>> GetAllAsync(CancellationToken ct)
        {
            var items = await _repo.GetAllAsync(ct);
            var result = items.Select(p => new ProductResponseDto { Id = p.Id, Name = p.Name, Price = p.Price });
            return Ok(result);
        }

        // PUBLIC_INTERFACE
        /// <summary>
        /// Retrieves a product by its unique identifier.
        /// </summary>
        /// <param name="id">Product identifier.</param>
        /// <returns>The product if found.</returns>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ProductResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Tags("Products")]
        [ActionName("GetProductById")]
        public async Task<ActionResult<ProductResponseDto>> GetByIdAsync(int id, CancellationToken ct)
        {
            var item = await _repo.GetByIdAsync(id, ct);
            if (item == null) return NotFound();
            return Ok(new ProductResponseDto { Id = item.Id, Name = item.Name, Price = item.Price });
        }

        // PUBLIC_INTERFACE
        /// <summary>
        /// Creates a new product.
        /// </summary>
        /// <param name="dto">Product details.</param>
        /// <returns>The created product.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ProductResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Tags("Products")]
        [ActionName("CreateProduct")]
        public async Task<ActionResult<ProductResponseDto>> CreateAsync([FromBody] CreateProductDto dto, CancellationToken ct)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var model = new Product
            {
                Name = dto.Name.Trim(),
                Price = dto.Price
            };

            var created = await _repo.CreateAsync(model, ct);
            var response = new ProductResponseDto { Id = created.Id, Name = created.Name, Price = created.Price };

            return CreatedAtAction(nameof(GetByIdAsync), new { id = created.Id }, response);
        }

        // PUBLIC_INTERFACE
        /// <summary>
        /// Updates an existing product.
        /// </summary>
        /// <param name="id">Product identifier.</param>
        /// <param name="dto">New values.</param>
        /// <returns>No content on success.</returns>
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Tags("Products")]
        [ActionName("UpdateProduct")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] UpdateProductDto dto, CancellationToken ct)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var ok = await _repo.UpdateAsync(id, new Product
            {
                Id = id,
                Name = dto.Name.Trim(),
                Price = dto.Price
            }, ct);

            if (!ok) return NotFound();
            return NoContent();
        }

        // PUBLIC_INTERFACE
        /// <summary>
        /// Deletes a product by its identifier.
        /// </summary>
        /// <param name="id">Product identifier.</param>
        /// <returns>No content on success.</returns>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Tags("Products")]
        [ActionName("DeleteProduct")]
        public async Task<IActionResult> DeleteAsync(int id, CancellationToken ct)
        {
            var ok = await _repo.DeleteAsync(id, ct);
            if (!ok) return NotFound();
            return NoContent();
        }
    }
}
