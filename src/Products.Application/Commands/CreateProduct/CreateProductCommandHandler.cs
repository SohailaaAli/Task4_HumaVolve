using MediatR;
using Microsoft.Extensions.Logging;
using Products.Application.DTOs;
using Products.Application.Interfaces;
using Products.Domain.Entities;

namespace Products.Application.Commands.CreateProduct;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductDto>
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<CreateProductCommandHandler> _logger;

    public CreateProductCommandHandler(
        IProductRepository productRepository,
        ILogger<CreateProductCommandHandler> logger)
    {
        _productRepository = productRepository;
        _logger = logger;
    }

    public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        _logger.LogDebug(
            "Handling CreateProductCommand. Name: {ProductName}, Price: {ProductPrice}",
            request.Name,
            request.Price);

        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Price = request.Price
        };

        try
        {
            await _productRepository.AddAsync(product, cancellationToken);

            _logger.LogInformation(
                "Product saved to database successfully. ProductId: {ProductId}",
                product.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to save product to database. Name: {ProductName}",
                request.Name);
            throw;
        }

        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price
        };
    }
}
