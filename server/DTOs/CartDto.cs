using System.Collections.Generic;
using e_commerce.Entities;

namespace e_commerce.DTOs
{
  public class CartDto
  {
    public decimal TotalPrice { get; set; }
    public ICollection<CartItemDto> Items { get; set; }
  }
}