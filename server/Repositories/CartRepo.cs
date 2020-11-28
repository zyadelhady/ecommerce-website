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
      .FirstOrDefaultAsync();
    }

    public async Task<CartDto> GetUserCartAsync(Guid userId)
    {
      return await _context.Carts.Where(c => c.UserId == userId)
      .Include(c => c.Items)
      .ProjectTo<CartDto>(_maper.ConfigurationProvider)
      .FirstOrDefaultAsync();
    }

    public void UpdateCart(Cart cart)
    {
      _context.Entry(cart).State = EntityState.Modified;
    }


  }
}