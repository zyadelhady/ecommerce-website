using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace e_commerce.DTOs
{
  public record CreateProductDto(
    [Required] string Name,
    [Required] decimal Price,
    [Required] int Quantity,
    [Required] string Description,
    [Required] IEnumerable<string> Images,
    [Required] IEnumerable<string> Colors,
    [Required] IEnumerable<string> Sizes,
    [Required] string CollectionName
    );
}