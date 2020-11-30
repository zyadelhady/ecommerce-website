using e_commerce.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace e_commerce.Entities
{
    [Table("OrderItems")]
    public class OrderItem : ShoppingItem
    {
        public int OrderId { get; set; }

    }
}
