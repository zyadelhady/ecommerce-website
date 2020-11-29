namespace e_commerce.DTOs
{
  public class CartItemDto
  {
    public int Id { get; set; }
    public int Quantity { get; set; }
    public string Color { get; set; }
    public string Size { get; set; }
    public CartProductDto Product { get; set; }
  }
}