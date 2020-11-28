using System;
using System.Collections.Generic;

namespace e_commerce.Entities.Base
{
  public class Shopping<TItem> where TItem : ShoppingItem
  {
    public int Id { get; set; }
    public Guid UserId { get; set; }
    public decimal TotalPrice { get; set; }
    public ICollection<TItem> Items { get; set; }
  }
}