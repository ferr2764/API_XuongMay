using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Threading.Tasks;
using XuongMay.Contract.Repositories.Entity;
using XuongMay.Contract.Services.Interface;
using XuongMay.ModelViews.CategoryModelViews;

namespace XuongMayBE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET: api/category
        /// <summary>
        /// Get all categories.
        /// </summary>
        /// <returns>A list of all categories.</returns>
        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(categories);
        }

        // GET: api/category/{id}
        /// <summary>
        /// Get a category by ID.
        /// </summary>
        /// <param name="id">The ID of the category.</param>
        /// <returns>The category details.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(string id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }

        // POST: api/category
        /// <summary>
        /// Create a new category.
        /// </summary>
        /// <param name="category">The category details to create.</param>
        /// <returns>The created category details.</returns>
        [Authorize(Roles = "Manager")]
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryModelView category)
        {
            if (category == null)
            {
                return BadRequest();
            }

            var createdCategory = await _categoryService.CreateCategoryAsync(category);
            return CreatedAtAction(nameof(GetCategory), new { id = createdCategory.Id }, createdCategory);
        }

        // PUT: api/category/{id}
        /// <summary>
        /// Update an existing category.
        /// </summary>
        /// <param name="id">The ID of the category to update.</param>
        /// <param name="category">The updated category details.</param>
        /// <returns>No content if the update is successful.</returns>
        [Authorize(Roles = "Manager")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(string id, [FromBody] Category category)
        {
            // Check if the provided id and the category.Id match and are valid ObjectIds
            if (!ObjectId.TryParse(id, out var objectId) || objectId != category.Id)
            {
                return BadRequest("Invalid ID format or ID mismatch.");
            }

            // Update the category using the service
            var updatedCategory = await _categoryService.UpdateCategoryAsync(id, category);
            if (updatedCategory == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        // PATCH: api/category/{id}
        /// <summary>
        /// Mark a category as deleted (e.g., by setting its status to "Unavailable").
        /// </summary>
        /// <param name="id">The ID of the category to "delete".</param>
        /// <returns>No content if the operation is successful.</returns>
        [Authorize(Roles = "Manager")]
        [HttpPatch("{id}")]
        public async Task<IActionResult> DeleteCategory(string id)
        {
            var isUnavailable = await _categoryService.DeleteCategoryAsync(id);
            if (!isUnavailable)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
