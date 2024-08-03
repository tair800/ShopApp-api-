using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopApp_API_.Apps.AdminApp.Dtos.ProductDto;
using ShopApp_API_.Data;
using ShopApp_API_.Entities;

namespace ShopApp_API_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ShopAppDbContext _context;
        private readonly IMapper _mapper;

        public ProductController(ShopAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int? id)
        {
            if (id == null) return BadRequest();
            var existProduct = await _context.Products
                .Include(p => p.Category)
                .ThenInclude(p => p.Products)
                .Where(p => !p.isDelete)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (existProduct == null) return NotFound();

            return Ok(_mapper.Map<ProductReturnDto>(existProduct));
        }

        [HttpGet]
        public async Task<IActionResult> Get(string search, int page = 1)
        {
            var query = _context.Products
                .Where(p => !p.isDelete);

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p => p.Name.ToLower().Contains(search.ToLower()));
            }

            ProductListDto listDto = new()
            {

                Page = page,
                TotalCount = query.Count(),
                Items = await query.Skip((page - 1) * 2).Take(2)
                .Select(p => new ProductListItemDto()
                {
                    Id = p.Id,
                    Name = p.Name,
                    CurrentPrice = p.CurrentPrice,
                    SalePrice = p.SalePrice,
                    CreatedDate = p.CreatedDate,
                    UpdatedDate = p.UpdatedDate,
                    Category = new()
                    {
                        Name = p.Category.Name,
                        ProductsCount = p.Category.Products.Count()
                    }
                })
                .ToListAsync()
            };
            return Ok(listDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateDto productCreateDto)
        {
            if (!await _context.Categories.AnyAsync(c => !c.isDelete && c.Id == productCreateDto.CategoryId))
                return StatusCode(409);

            Product product = new();

            product.Name = productCreateDto.Name;
            product.SalePrice = productCreateDto.SalePrice;
            product.CurrentPrice = productCreateDto.CurrentPrice;
            product.CategoryId = productCreateDto.CategoryId;

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int? id, ProductUpdateDto productUpdateDto)
        {
            if (id is null) return StatusCode(StatusCodes.Status400BadRequest);

            var existProduct = await _context.Products
                .Where(p => !p.isDelete)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (existProduct == null) return NotFound();

            if (!await _context.Categories.AnyAsync(c => !c.isDelete && c.Id == productUpdateDto.CategoryId))
                return StatusCode(409);

            existProduct.Name = productUpdateDto.Name;
            existProduct.SalePrice = productUpdateDto.SalePrice;
            existProduct.CurrentPrice = productUpdateDto.CurrentPrice;
            existProduct.CategoryId = productUpdateDto.CategoryId;
            await _context.SaveChangesAsync();
            return StatusCode(StatusCodes.Status204NoContent);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(int id, bool status)
        {
            var existProduct = await _context.Products
                .Where(p => !p.isDelete)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (existProduct == null) return NotFound();
            existProduct.isDelete = status;
            await _context.SaveChangesAsync();
            return StatusCode(StatusCodes.Status204NoContent);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var existProduct = await _context.Products
                .Where(p => !p.isDelete)
                .FirstOrDefaultAsync(x => x.Id == id);


            if (existProduct == null) return NotFound();
            _context.Products.Remove(existProduct);

            await _context.SaveChangesAsync();
            return StatusCode(StatusCodes.Status204NoContent);
        }
    }
}
