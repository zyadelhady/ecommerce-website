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
    void DeleteCartItem(Cart cart, CartItem item);
    Task<Cart> GetCartAsync(Guid userId);
    Task<CartDto> GetUserCartAsync(Guid userId);

    CartItem GetCartItem(Cart cart, AddCartItemDto itemDto);
    CartItem GetCartItem(Cart cart, int id);
    void UpdateCart(Cart cart);
  }
}