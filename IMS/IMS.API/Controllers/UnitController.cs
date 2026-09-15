using IMS.Application.Features.Unit.Command;
using IMS.Application.Features.Units.Query;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnitController : AuthenticatedController
    {
        private readonly IMediator _mediator;

        public UnitController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> GetUnits()
        {
            var result = await _mediator.Send(
                new GetUnitQuery());

            return Ok(new
            {
                success = true,
                data = result
            });
        }

        // GET: api/Unit/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUnitById(int id)
        {
            var result = await _mediator.Send(
                new GetUnitByIdQuery
                {
                    Id = id
                });

            if (result == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Unit not found."
                });
            }

            return Ok(new
            {
                success = true,
                data = result
            });
        }

        // POST: api/Unit
        [HttpPost]
        public async Task<IActionResult> CreateUnit(
            [FromBody] CreateUnitCommand command)
        {
            var result = await _mediator.Send(command);

            return Ok(new
            {
                success = true,
                message = "Unit created successfully.",
                data = result
            });
        }

        // PUT: api/Unit/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUnit(
            int id,
            [FromBody] UpdateUnitCommand command)
        {
            command.Id = id;

            var result = await _mediator.Send(command);

            if (result == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Unit not found."
                });
            }

            return Ok(new
            {
                success = true,
                message = "Unit updated successfully.",
                data = result
            });
        }

        // DELETE: api/Unit/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUnit(int id)
        {
            var result = await _mediator.Send(
                new DeleteUnitCommand
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
