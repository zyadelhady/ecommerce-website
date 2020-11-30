using AutoMapper;
using e_commerce.Data;
using e_commerce.DTOs;
using e_commerce.Entities;
using e_commerce.Extensions;
using e_commerce.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
        [HttpPost]
        public async Task<ActionResult<CartDto>> AddItemToCart(AddCartItemDto itemDto)
        {
            var cart = await _unitOfWork.CartRepo.GetCartAsync(User.GetUserId());

            if (cart == null)
            {
                return NotFound("No cart for this user");
            }

            var item = _unitOfWork.CartRepo.GetCartItem(cart, itemDto);

            if (item == null)
            {
                var newItem = new CartItem
                {
                    ProductId = itemDto.ProductId,
                    Quantity = 1,
                    Color = itemDto.Color,
                    Size = itemDto.Size
                };

                cart.Items.Add(newItem);
            }
            else
            {
                item.Quantity++;
            }


            _unitOfWork.CartRepo.UpdateCart(cart);

            if (await _unitOfWork.Complete())
            {
                return _mapper.Map<CartDto>(cart);
            }

            return BadRequest("Couldn't add the item");
        }


        [Authorize]
        [HttpPut("item/{id}")]
        public async Task<ActionResult<CartDto>> UpdateCartItem(int id, updateCartItemDto itemDto)
        {
            var cart = await _unitOfWork.CartRepo.GetCartAsync(User.GetUserId());
            if (cart == null)
            {
                return NotFound("No cart for this user");
            }

            var item = _unitOfWork.CartRepo.GetCartItem(cart, id);

            if (item == null)
            {
                return BadRequest("you  should add the item to the cart first");
            }

            if (item.Quantity < 1)
            {
                return BadRequest("Quantity must be greater than 0");
            }

            item.Quantity = itemDto.Quantity;

            _unitOfWork.CartRepo.UpdateCart(cart);

            if (await _unitOfWork.Complete())
            {
                return NoContent();
            }

            return BadRequest("Couldn't update the item");
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCartItem(int id)
        {
            var cart = await _unitOfWork.CartRepo.GetCartAsync(User.GetUserId());
            if (cart == null)
            {
                return NotFound("No cart for this user");
            }

            var item = _unitOfWork.CartRepo.GetCartItem(cart, id);

            if (item == null)
            {
                return BadRequest("this item isn't in your cart");
            }

            _unitOfWork.CartRepo.DeleteCartItem(cart, item);

            _unitOfWork.CartRepo.UpdateCart(cart);

            if (await _unitOfWork.Complete())
            {
                return NoContent();
            }

            return BadRequest("Couldn't delete the item");
        }

        [Authorize]
        [HttpDelete]
        public async Task<ActionResult> DeleteItems()
        {
            var cart = await _unitOfWork.CartRepo.GetCartAsync(User.GetUserId());
            if (cart == null)
            {
                return NotFound("No cart for this user");
            }

            _context.RemoveRange(cart.Items);
            cart.Items.Clear();

            cart.TotalPrice = 0;

            _unitOfWork.CartRepo.UpdateCart(cart);

           await _unitOfWork.Complete();

            return NoContent();

        }
    }
}