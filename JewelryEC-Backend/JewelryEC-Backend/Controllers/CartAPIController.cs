using AutoMapper;
using JewelryEC_Backend.Data;
using JewelryEC_Backend.Models;
using JewelryEC_Backend.Models.CartItems.Dto;
using JewelryEC_Backend.Models.CartItems.Entities;
using JewelryEC_Backend.Models.Carts.Dto;
using JewelryEC_Backend.Models.Carts.Entities;
using JewelryEC_Backend.Models.Catalogs.Dto;
using JewelryEC_Backend.Service;
using JewelryEC_Backend.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace JewelryEC_Backend.Controllers
{
    [Route("api/user/{id}/cart/items")]
    [ApiController]
    [Authorize]
    public class CartAPIController : Controller
    {
        private readonly IMapper _mapper;
        private ResponseDto _response;
        private ICartService _cartService;

        public CartAPIController
            (IMapper mapper,  ICartService cartService, IProductService productService)
        {
            _mapper = mapper;
            _cartService = cartService;
            _response = new ResponseDto();

        }

        [HttpGet]
        public async Task<ActionResult<ResponseDto>> GetCartItems(Guid id)
        {
            try
             {
                    if (!checkValidUserId(id))
                    {
                            _response.IsSuccess = false;
                            _response.Message = "UserId is invalid";
                            return BadRequest(_response);
                    }
                    var cart = _cartService.GetDetailCart(id);
                    if (cart is null)
                    {
                        _response.IsSuccess = false;
                        _response.Message = "Cart doesn't exist !!!";
                        return StatusCode(404, _response);
                    }
                    else
                    {
                        var getCartDto = _mapper.Map<GetCartResponseDto>(cart);
                   
                        for (int i = 0; i < getCartDto.Items.Count; i++)
                        {
                            var productDetail = _cartService.GetCartItemDetail(getCartDto.Items[0].ProductItemId);
                            getCartDto.Items[i].NameProduct = _cartService.GetCartItemName(productDetail.ProductId);
                            if (productDetail.DiscountPrice is not null)
                            {
                                getCartDto.Items[i].DiscountPrice = productDetail.DiscountPercent.Value;
                                getCartDto.TotalPrice += productDetail.DiscountPrice * getCartDto.Items[0].Count;
                            }
                            else
                            {
                                getCartDto.TotalPrice += productDetail.Price * getCartDto.Items[0].Count;
                            }
                            getCartDto.Items[i].SalePrice = productDetail.Price;

                            getCartDto.Items[i].Description = productDetail.Description;
                            getCartDto.Items[i].Image = productDetail.Image;

                        }
                    _response.Result = getCartDto;
                        return Ok(_response);
                    }
                
                
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.ToString();
                return StatusCode(500, _response);
            }
        }

        private bool checkValidUserId(Guid userId)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim != userId.ToString())
            {
                return false;
            }
            return true;
        }

        [HttpPost]
        public async Task<ActionResult<ResponseDto>> CartUpsert(Guid id, [FromBody]CreateCartItemDto cartItemDto)
        {
            if (!checkValidUserId(id))
            {
                _response.IsSuccess = false;
                _response.Message = "UserId is invalid.";
                return BadRequest(_response);
            }
          
            CartItem cartItem = _mapper.Map<CartItem>(cartItemDto);

            try
            {
                var cartItemNew = _cartService.CartUpSert(id, cartItem);
                if( cartItemNew == null )
                {
                    _response.IsSuccess = false;
                    _response.Message = $"ProductItem {cartItemDto.ProductItemId} doesn't exist or Quantity is invalid. ";
                    return BadRequest(_response);
                }
                _response.Result = _mapper.Map<CreateCartItemResponseDto>(cartItemNew);
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.ToString();
                return StatusCode(500, _response);
            }
  
        }

        [HttpDelete("{productItemId}")]
        public async Task<ActionResult<ResponseDto>> CartItemDelete(Guid id , Guid productItemId)
        {
            
            try
            {
                if (!checkValidUserId(id))
                {
                    _response.IsSuccess = false;
                    _response.Message = "UserId is invalid.";
                    return BadRequest(_response);
                }
               
                var result = _cartService.DeleteCartItem(id, productItemId);
                if (result == false)
                {
                    _response.IsSuccess = false;
                    _response.Message = $"Product Id {productItemId} doesn't exist or some errors occured.";
                    return BadRequest(_response);
                }      
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.ToString();
                return StatusCode(500, _response);
            }

        }

    }
}
