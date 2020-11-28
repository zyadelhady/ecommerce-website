namespace e_commerce.DTOs
{

  public record AddCartItemDto(
      int ProductId,
      string Color,
      string Size
      );
}