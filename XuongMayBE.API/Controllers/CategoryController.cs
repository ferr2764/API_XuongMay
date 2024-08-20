using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
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
        public async Task<IActionResult> GetAllCategories(int page = 1, int pageSize = 5)
        {
            var categories = await _categoryService.GetCategoriesByPageAsync(page, pageSize);
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

            try
            {
                var createdCategory = await _categoryService.CreateCategoryAsync(category);
                return CreatedAtAction(nameof(GetCategory), new { id = createdCategory.Id }, createdCategory);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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
        public async Task<IActionResult> UpdateCategory(string id, [FromBody] UpdateCategoryModelView category)
        {
            if (!ObjectId.TryParse(id, out var objectId))
            {
                return BadRequest("Invalid ID format.");
            }


            try
            {
                var updatedCategory = await _categoryService.UpdateCategoryAsync(id, category);
                if (updatedCategory == null)
                {
                    return NotFound("Category not found.");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PATCH: api/category/{id}
        /// <summary>
        /// Mark a category as deleted (e.g., by setting its status to "Unavailable").
        /// </summary>
        /// <param name="id">The ID of the category to "delete".</param>
        /// <returns>No content if the operation is successful.</returns>
        [Authorize(Roles = "Manager")]
        [HttpPatch("delete/{id}")]
        public async Task<IActionResult> DeleteCategory(string id)
        {
            if (!ObjectId.TryParse(id, out _))
            {
                return BadRequest("Invalid ID format.");
            }

            try
            {
                var isUnavailable = await _categoryService.DeleteCategoryAsync(id);
                if (!isUnavailable)
                {
                    return NotFound("Category not found.");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
