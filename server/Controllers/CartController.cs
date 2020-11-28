using System.Threading.Tasks;
using AutoMapper;
using e_commerce.Data;
using e_commerce.DTOs;
using e_commerce.Entities;
using e_commerce.Extensions;
using e_commerce.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace e_commerce.Controllers
{
  public class CartController : BaseApiController
  {
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly DataContext _context;
    public CartController(IUnitOfWork unitOfWork, ICollectionRepo collectionRepo, IMapper mapper, DataContext context)
    {
      _context = context;
      _mapper = mapper;
      _unitOfWork = unitOfWork;
    }


    [Authorize]
    [HttpGet]
    public async Task<ActionResult<CartDto>> GetCart()
    {
      var cart = await _unitOfWork.CartRepo.GetUserCartAsync(User.GetUserId());
      return cart == null ? NotFound("No cart for this user") : cart;
    }

    [Authorize]
    [HttpPost("add-to-cart")]
    public async Task<ActionResult<CartDto>> AddItemToCart(AddCartItemDto itemDto)
    {
      var cart = await _unitOfWork.CartRepo.GetCartAsync(User.GetUserId());

      var newItem = new CartItem
      {
        ProductId = itemDto.ProductId,
        Quantity = 1,
        Color = itemDto.Color,
        Size = itemDto.Size
      };

      cart.Items.Add(newItem);

      _unitOfWork.CartRepo.UpdateCart(cart);

      if (await _unitOfWork.Complete()) return _mapper.Map<CartDto>(cart);
      return BadRequest("Couldn't add the item");
    }
  }
}