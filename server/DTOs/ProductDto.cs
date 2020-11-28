using System.Collections.Generic;

namespace e_commerce.DTOs
{
  public record ProductDto(
    string Name,
    string Slug,
    decimal Price,
    int Quantity,
    string Description,
    IEnumerable<string> Images,
    IEnumerable<string> Colors,
    IEnumerable<string> Sizes,
    string CollectionName
    );
}