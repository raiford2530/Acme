using Acme.Shop.Application.Products.Commands;
using Acme.Shop.Application.Products.Queries;
using Acme.Shop.Contracts.Products;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Acme.Shop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<ProductResponse>> Create([FromBody] CreateProductRequest request, CancellationToken ct)
        {
            var result = await _mediator.Send(new CreateProductCommand(request), ct);
            return CreatedAtAction(nameof(GetById), new { id = result.Id, version = "1.0" }, result);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ProductResponse>> GetById([FromRoute] Guid id, CancellationToken ct)
        {
            var res = await _mediator.Send(new GetProductByIdQuery(id), ct);
            return res is null ? NotFound() : Ok(res);
        }
    }
}
