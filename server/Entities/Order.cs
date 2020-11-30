using e_commerce.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace e_commerce.Entities
{
    public class Order : Shopping<OrderItem>
    {
    }
}
