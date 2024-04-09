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
        private readonly ICacheService _cacheService;
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;
        private ResponseDto _response;


        public CartAPIController(ICacheService cacheService, AppDbContext appDbContext, IMapper mapper)
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
                var cacheData = _cacheService.GetData<Cart>("cart:user-"+id);
                if (cacheData != null)
                {
                    _response.Result = _mapper.Map<GetCartDto>(cacheData);
                    
                }
                else
                {
                    cacheData = _appDbContext.Carts
                               .Where(c => c.UserId == id)  // Lọc theo UserId
                               .OrderBy(c => c.CreatedAt)   // Sắp xếp theo CreatedAt tăng dần
                               .FirstOrDefault();
                    // set expiry time
                    var expiryTime = DateTimeOffset.Now.AddSeconds(30);
                    _cacheService.SetData("cart:user-" + id, cacheData, expiryTime);
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
            GetCartItemDto cartItemDto = new GetCartItemDto();
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
                    cartItemDto.Id= Guid.NewGuid();
                    cartItemDto.CartId = cart.Id;
                    cartItemDto.ProductId = cartDto.CartItemDto.ProductId;
                    cartItemDto.Count  = cartDto.CartItemDto.Count;
                    _appDbContext.CartItems.Add(_mapper.Map<CartItem>(cartItemDto));
                    await _appDbContext.SaveChangesAsync();
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
                        cartItemDto.Id = Guid.NewGuid();
                        cartItemDto.CartId = cartFromDb.Id;
                        cartItemDto.ProductId = cartDto.CartItemDto.ProductId;
                        cartItemDto.Count = cartDto.CartItemDto.Count;
                        _appDbContext.CartItems.Add(_mapper.Map<CartItem>(cartItemDto));
                        await _appDbContext.SaveChangesAsync();
                    }
                    else
                    {
                        //update count in cart details
                        cartItemDto.Id = Guid.NewGuid();
                        cartItemDto.CartId = cartFromDb.Id;
                        cartItemDto.ProductId = cartDto.CartItemDto.ProductId;
                        cartItemDto.Count += cartDto.CartItemDto.Count;
                      
                        _appDbContext.CartItems.Update(_mapper.Map<CartItem>(cartItemDto));
                        await _appDbContext.SaveChangesAsync();
                    }
                }
                _response.Result = cartItemDto;
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
