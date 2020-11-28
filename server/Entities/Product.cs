using System;
using System.Collections.Generic;

namespace e_commerce.Entities
{


  public class Product
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Slug { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    public List<string> Images { get; set; }
    public List<string> Colors { get; set; }
    public List<string> Sizes { get; set; }
    public int Quantity { get; set; }
    public Collection Collection { get; set; }
    public int CollectionId { get; set; }
  }
}