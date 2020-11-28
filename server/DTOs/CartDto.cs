using System.Collections.Generic;
using e_commerce.Entities;

namespace e_commerce.DTOs
{
  public record CartDto(
     decimal TotalPrice,
     ICollection<CartItem> Items
  );
}