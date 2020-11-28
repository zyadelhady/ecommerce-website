using System;
using System.Threading.Tasks;
using e_commerce.DTOs;
using e_commerce.Entities;

namespace e_commerce.Interfaces
{
  public interface ICartRepo
  {
    void AddCart(Cart cart);
    void DeleteCart(Cart cart);
    Task<Cart> GetCartAsync(Guid userId);
    Task<CartDto> GetUserCartAsync(Guid userId);

    void UpdateCart(Cart cart);
  }
}