using IMS.Application.Features.Categories.Command.CreateCategory;
using IMS.Application.Features.Categories.Command.DeleteCategories;
using IMS.Application.Features.Categories.Command.UpdateCategories;
using IMS.Application.Features.Categories.Query.GetCategories;
using IMS.Application.Features.Categories.Query.GetCategoryById;
using IMS.Application.Features.Products.Command;
using IMS.Application.Features.Products.Query;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : AuthenticatedController
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var result = await _mediator.Send(
                new GetProductQuery());

            return Ok(new
            {
                success = true,
                data = result
            });
        }

        // GET: api/Product/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var result = await _mediator.Send(
                new GetProductByIdQuery
                {
                    Id = id
                });

            if (result == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Product not found."
                });
            }

            return Ok(new
            {
                success = true,
                data = result
            });
        }

        // POST: api/Product
        [HttpPost]
        public async Task<IActionResult> CreateProduct(
            [FromForm] CreateProductCommand command)
        {
            var result = await _mediator.Send(command);

            return Ok(new
            {
                success = true,
                message = "Product created successfully.",
                data = result
            });
        }

        // PUT: api/Product/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(
            int id,
            [FromForm] ProductCommand command)
        {
            command.Id = id;

            var result = await _mediator.Send(command);

            if (result == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Product not found."
                });
            }

            return Ok(new
            {
                success = true,
                message = "Product updated successfully.",
                data = result
            });
        }

        // DELETE: api/Product/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var result = await _mediator.Send(
                new DeleteProductCommand
                {
                    Id = id
                });

            if (!result)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Product not found."
                });
            }

            return Ok(new
            {
                success = true,
                message = "Product deleted successfully."
            });
        }
    }
}
