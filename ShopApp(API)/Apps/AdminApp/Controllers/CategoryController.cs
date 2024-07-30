using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopApp_API_.Apps.AdminApp.Dtos.CategoryDto;
using ShopApp_API_.Data;
using ShopApp_API_.Entities;

namespace ShopApp_API_.Apps.AdminApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {

        private readonly ShopAppDbContext _context;

        public CategoryController(ShopAppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int? id)
        {
            if (id is null) return BadRequest();

            var existCategory = await _context.Categories
                .Where(p => !p.isDelete)
                .Select(c => new CategoryReturnDto()
                {
                    Id = c.Id,
                    Name = c.Name,
                    CreatedDate = c.CreatedDate,
                    UpdatedDate = c.UpdatedDate,
                    ImageUrl = "http://localhost:5036/images/" + c.Image
                })
                .FirstOrDefaultAsync(p => p.Id == id);

            return Ok(existCategory);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreateDto categoryCreateDto)
        {
            var isExist = await _context.Categories
                .AnyAsync(p => !p.isDelete && p.Name.ToLower() == categoryCreateDto.Name.ToLower());

            if (isExist) return StatusCode(409);

            if (categoryCreateDto.Image is null)
                return BadRequest();

            if (categoryCreateDto.Image.ContentType.Contains("images/"))
                return BadRequest();

            if (categoryCreateDto.Image.Length / 1024 > 1000)
                return BadRequest();

            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(categoryCreateDto.Image.FileName);
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);
            using FileStream fileStream = new(path, FileMode.Create);
            await categoryCreateDto.Image.CopyToAsync(fileStream);


            var file = categoryCreateDto.Image;
            Category category = new()
            {
                Name = categoryCreateDto.Name.Trim(),
                Image = fileName,
            };

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            return StatusCode(201);

        }



        //public async Task<string> SaveFilesAsync(IFormFile file)
        //{
        //    var filePath = Path.Combine("wwwroot/images/", file.FileName);
        //    using (var stream = new FileStream(filePath, FileMode.Create))
        //    {
        //        await file.CopyToAsync(stream);
        //    }
        //    return $"{file.FileName}";
        //}
    }
}
