using System.ComponentModel.DataAnnotations.Schema;
using e_commerce.Entities.Base;

namespace e_commerce.Entities
{
  [Table("CartItems")]
  public class CartItem : ShoppingItem
  {
    public int CartId { get; set; }
  }
}