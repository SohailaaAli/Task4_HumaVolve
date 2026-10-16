using MediatR;
using Products.Application.DTOs;

namespace Products.Application.Commands.CreateProduct;

public record CreateProductCommand(string Name, decimal Price) : IRequest<ProductDto>;
