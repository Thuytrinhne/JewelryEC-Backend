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
        private readonly IRedisShoppingCartService _cacheService;
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;
        private ResponseDto _response;


        public CartAPIController(IRedisShoppingCartService cacheService, AppDbContext appDbContext, IMapper mapper)
        {
            _cacheService = cacheService;
            _appDbContext = appDbContext;
            _mapper = mapper;
            _response = new ResponseDto();
        }

        [HttpGet]
        [Route("user/{id}/carts")]
        public async Task<ActionResult<ResponseDto>> Get(string id)
        {
            try
            {
              
                // check cache data
                var cacheData = _cacheService.GetData(id);
                if (cacheData != null && cacheData.Count >0 )
                {
                    GetCartDto getCart = new GetCartDto();
                    getCart.UserId = id;
                    //getCart.Items = cacheData;
                    getCart.Items = new List<GetCartItemDto>();
                    foreach (var item in cacheData)
                    {
                        getCart.Items.Add(new GetCartItemDto { ProductId = item.Key, Count = item.Value });
                    }

                    _response.Result = getCart;
                    
                }
                else
                {
                           var CartFrmDb =_appDbContext.Carts
                                   .Where(c => c.UserId == id && c.IsPayed == 0) 
                                   .FirstOrDefault();
                    
                           var  CartItems =  _appDbContext.CartItems
                                            .Where(c => c.CartId == CartFrmDb.Id)
                                            .ToList();

                    // set CACHE
                    if (CartItems.Count == 0)
                    {
                        // set null
                    }
                    else
                    {
                        foreach (var item in CartItems)
                        {
                            _cacheService.SetData(id, item.ProductId, item.Count);
                        }
                    }
                    //var expiryTime = DateTimeOffset.Now.AddSeconds(30);

                }
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
                return StatusCode(500, _response);
            }
      



        }

        [HttpPost("CartUpsert")]
        public async Task<ActionResult<ResponseDto>> CartUpsert(CreateCartDto cartDto)
        {
            CartItem cartItem = _mapper.Map<CartItem>(cartDto.CartItemDto);

            try
            {
                var cartFromDb = await _appDbContext.Carts.AsNoTracking()
                    .FirstOrDefaultAsync(u => u.UserId == cartDto.UserId && u.IsPayed==0);
                if (cartFromDb == null)
                {
                    //create cart new and details 
                    Cart cart = _mapper.Map<Cart>(cartDto);
                    cart.Id = Guid.NewGuid();
                    _appDbContext.Carts.Add(cart);
                    await _appDbContext.SaveChangesAsync();
                    // create cart item     
                    cartItem.CartId = cartFromDb.Id;
                    cartItem.Id = Guid.NewGuid();
                    _appDbContext.CartItems.Add(cartItem);
                    await _appDbContext.SaveChangesAsync();
                    // set data into cache
                    _cacheService.SetData(cartDto.UserId, cartItem.ProductId, cartItem.Count);
                }
                else
                {
                    //if header is not null
                    //check if details has same product
                    var cartDetailsFromDb = await _appDbContext.CartItems.AsNoTracking().FirstOrDefaultAsync(
                        u => u.ProductId == cartDto.CartItemDto.ProductId &&
                        u.CartId == cartFromDb.Id);
                    if (cartDetailsFromDb == null)
                    {
                        //create cartdetails
                        cartItem.CartId = cartFromDb.Id;
                        cartItem.Id = Guid.NewGuid();   
                        _appDbContext.CartItems.Add(cartItem);
                        await _appDbContext.SaveChangesAsync();
                        _cacheService.SetData(cartDto.UserId, cartItem.ProductId, cartItem.Count);
                    }
                    else
                    {
                        //update count in cart details
                        cartDetailsFromDb.Count += cartDto.CartItemDto.Count;
                        _appDbContext.CartItems.Update(cartDetailsFromDb);
                        await _appDbContext.SaveChangesAsync();
                        _cacheService.SetData(cartDto.UserId, cartDetailsFromDb.ProductId, cartItem.Count);
                        cartItem.Count = cartDetailsFromDb.Count;
                    }


                }

                _response.Result = _mapper.Map<CreateCartItemResponseDto>(cartItem);
                return _response;
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
