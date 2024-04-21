using AutoMapper;
using JewelryEC_Backend.Data;
using JewelryEC_Backend.Migrations;
using JewelryEC_Backend.Models;
using JewelryEC_Backend.Models.CartItems.Dto;
using JewelryEC_Backend.Models.CartItems.Entities;
using JewelryEC_Backend.Models.Carts.Dto;
using JewelryEC_Backend.Models.Carts.Entities;
using JewelryEC_Backend.Models.Catalogs.Dto;
using JewelryEC_Backend.Service;
using JewelryEC_Backend.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JewelryEC_Backend.Controllers
{
    [Route("api")]
    [ApiController]
    public class CartAPIController : Controller
    {
        private readonly IMapper _mapper;
        private ResponseDto _response;
        private ICartService _cartService;
       
        public CartAPIController(IMapper mapper,  ICartService cartService)
        {
            _mapper = mapper;
            _cartService = cartService;
            _response = new ResponseDto();

        }

        [HttpGet("user/{id}/cart/items")]
        public async Task<ActionResult<ResponseDto>> GetCartItems(Guid id)
        {
            try
            {
                var cart = _cartService.GetDetailCart(id);
                if(cart == null)
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessages = new List<string>() {"Giỏ hàng rỗng !!!"};
                    return StatusCode(404, _response);
                }
                else
                {
                    GetCartDto getCartDto = _mapper.Map<GetCartDto>(cart);
                    getCartDto.Items = _mapper.Map<List<GetCartItemDto>>(cart.cartItems);
                    _response.Result = getCartDto;
                    return Ok(_response);
                }
                
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
                return StatusCode(500, _response);
            }
        }
        [HttpPost("user/{id}/cart/items")]
        public async Task<ActionResult<ResponseDto>> CartUpsert(Guid id, [FromBody]CreateCartItemDto cartItemDto)
        {
            CartItem cartItem = _mapper.Map<CartItem>(cartItemDto);

            try
            {
                var cartItemNew = _cartService.CartUpSert(id, cartItem);
                _response.Result = _mapper.Map<CreateCartItemResponseDto>(cartItemNew);
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
                return StatusCode(500, _response);
            }
  
        }

    }
}