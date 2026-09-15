using IMS.Application.Features.Categories.Command.CreateCategory;
using IMS.Application.Features.Categories.Command.DeleteCategories;
using IMS.Application.Features.Categories.Command.UpdateCategories;
using IMS.Application.Features.Categories.Query.GetCategories;
using IMS.Application.Features.Categories.Query.GetCategoryById;
using IMS.Application.Features.Products.Command;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : AuthenticatedController
    {
        private readonly IMediator _mediator;

        public CategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            //var user = ClaimUserId;
            //var store = ClaimStoreId;
            var result = await _mediator.Send(
                new GetCategoriesQuery());

            return Ok(new
            {
                success = true,
                data = result
            });
        }

        // GET: api/Category/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var result = await _mediator.Send(
                new GetCategoryByIdQuery
                {
                    Id = id
                });

            if (result == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Category not found."
                });
            }

            return Ok(new
            {
                success = true,
                data = result
            });
        }

        // POST: api/Category
        [HttpPost]
        public async Task<IActionResult> CreateCategory(
            [FromBody] CreateCategoryCommand command)
        {
            var result = await _mediator.Send(command);

            return Ok(new
            {
                success = true,
                message = "Category created successfully.",
                data = result
            });
        }

        // PUT: api/Category/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(
            int id,
            [FromBody] UpdateCategoriesCommand command)
        {
            command.Id = id;

            var result = await _mediator.Send(command);

            if (result == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Category not found."
                });
            }

            return Ok(new
            {
                success = true,
                message = "Category updated successfully.",
                data = result
            });
        }

        // DELETE: api/Category/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var result = await _mediator.Send(
                new DeleteCategoryCommand
                {
                    Id = id
                });

            if (!result)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Category not found."
                });
            }

            return Ok(new
            {
                success = true,
                message = "Category deleted successfully."
            });
        }


    }
}
