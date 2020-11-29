using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using e_commerce.Data;
using e_commerce.DTOs;
using e_commerce.Entities;
using e_commerce.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace e_commerce.Repositories
{
  public class CartRepo : ICartRepo
  {
    private readonly DataContext _context;
    private readonly IMapper _maper;
    public CartRepo(DataContext context, IMapper maper)
    {
      _maper = maper;
      _context = context;

    }

    public void AddCart(Cart cart)
    {
      _context.Carts.Add(cart);
    }

    public void DeleteCart(Cart cart)
    {
      _context.Carts.Remove(cart);
    }

    public async Task<Cart> GetCartAsync(Guid userId)
    {
      return await _context.Carts.Where(c => c.UserId == userId)
      .Include(c => c.Items)
      .ThenInclude(p => p.Product)
      .FirstOrDefaultAsync();
    }

    public async Task<CartDto> GetUserCartAsync(Guid userId)
    {
      return await _context.Carts.Where(c => c.UserId == userId)
      .Include(c => c.Items)
      .ProjectTo<CartDto>(_maper.ConfigurationProvider)
      .FirstOrDefaultAsync();
    }

    public CartItem GetCartItem(Cart cart, AddCartItemDto itemDto)
    {
      return cart.Items.FirstOrDefault(i =>
          i.CartId == cart.Id
          && i.Color == itemDto.Color
          && i.Size == itemDto.Size
          && i.ProductId == itemDto.ProductId);
    }


    public CartItem GetCartItem(Cart cart, int id)
    {
      return cart.Items.FirstOrDefault(c => c.Id == id);
    }
    public void DeleteCartItem(Cart cart, CartItem item)
    {
      cart.Items.Remove(item);
    }

    public async void UpdateCart(Cart cart)
    {
      cart.TotalPrice = 0;


      foreach (CartItem item in cart.Items)
      {
        var Product = await _context.Products.FindAsync(item.ProductId);

        cart.TotalPrice += Product.Price * item.Quantity;
      }

      _context.Entry(cart).State = EntityState.Modified;
    }

  }
}