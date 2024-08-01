﻿using Microsoft.AspNetCore.Mvc;
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
                .Include(c => c.Products)
                .Where(p => !p.isDelete)

                .FirstOrDefaultAsync(p => p.Id == id);


            CategoryReturnDto category = new()
            {
                Id = existCategory.Id,
                Name = existCategory.Name,
                CreatedDate = existCategory.CreatedDate,
                UpdatedDate = existCategory.UpdatedDate,
                ImageUrl = "http://localhost:5036/images/" + existCategory.Image
            };

            return Ok(category);
        }

        [HttpGet]
        public async Task<IActionResult> Get(string search, int page = 1)
        {
            var query = _context.Categories
                .Where(p => !p.isDelete);

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(c => c.Name.ToLower().Contains(search.ToLower()));
            }

            CategoryListDto categoryListDto = new()
            {
                Page = page,
                TotalCount = query.Count(),
                Items = await query.Skip((page - 1) * 2).Take(2)
              .Select(c => new CategoryListItemDto()
              {
                  Id = c.Id,
                  Name = c.Name,
                  CreatedDate = c.CreatedDate,
                  UpdatedDate = c.UpdatedDate,

              }).ToListAsync()
            };

            return Ok(categoryListDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreateDto categoryCreateDto)
        {
            var isExist = await _context.Categories
                .AnyAsync(p => !p.isDelete && p.Name.ToLower() == categoryCreateDto.Name.ToLower());


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

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int? id, CategoryUpdateDto categoryUpdateDto)
        {
            if (id is null) return StatusCode(StatusCodes.Status400BadRequest);

            var existCategory = await _context.Categories
                .Where(c => !c.isDelete)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (existCategory == null) return NotFound();

            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(categoryUpdateDto.Image.FileName);
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);
            using FileStream fileStream = new(path, FileMode.Create);
            await categoryUpdateDto.Image.CopyToAsync(fileStream);

            existCategory.Name = categoryUpdateDto.Name;
            existCategory.Image = fileName;

            await _context.SaveChangesAsync();
            return StatusCode(StatusCodes.Status204NoContent);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(int? id, bool status)
        {
            if (id is null) return BadRequest();

            var existCategory = await _context.Categories
                .Where(c => !c.isDelete)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (existCategory == null) return NotFound();

            existCategory.isDelete = status;
            await _context.SaveChangesAsync();
            return StatusCode(StatusCodes.Status204NoContent);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return BadRequest();

            var existCategory = await _context.Categories
                .Where(_c => !_c.isDelete)
                .FirstOrDefaultAsync(_c => _c.Id == id);

            if (existCategory == null) return NotFound();

            _context.Categories.Remove(existCategory);
            await _context.SaveChangesAsync();

            return Ok(StatusCodes.Status200OK);

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
