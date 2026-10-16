using MediatR;
using Products.Application.DTOs;

namespace Products.Application.Queries.GetProducts;

public record GetProductsQuery() : IRequest<IReadOnlyList<ProductDto>>;
