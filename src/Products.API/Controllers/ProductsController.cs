using MediatR;
using Microsoft.AspNetCore.Mvc;
using Products.Application.Commands.CreateProduct;
using Products.Application.DTOs;
using Products.Application.Queries.GetProductById;
using Products.Application.Queries.GetProducts;

namespace Products.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(IMediator mediator, ILogger<ProductsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ProductDto>>> GetProducts(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting all products");

        var products = await _mediator.Send(new GetProductsQuery(), cancellationToken);

        _logger.LogInformation("Retrieved {ProductCount} products", products.Count);

        return Ok(products);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProductDto>> GetProductById(Guid id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting product with ID {ProductId}", id);

        var product = await _mediator.Send(new GetProductByIdQuery(id), cancellationToken);

        if (product is null)
        {
            _logger.LogWarning("Product with ID {ProductId} was not found", id);
            return NotFound();
        }

        _logger.LogInformation("Product {ProductId} retrieved successfully. Name: {ProductName}", id, product.Name);

        return Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult<ProductDto>> CreateProduct(
        [FromBody] CreateProductCommand command,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Creating product. Name: {ProductName}, Price: {ProductPrice}",
            command.Name,
            command.Price);

        var product = await _mediator.Send(command, cancellationToken);

        _logger.LogInformation(
            "Product created successfully. ProductId: {ProductId}, Name: {ProductName}",
            product.Id,
            product.Name);

        return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
    }
}
