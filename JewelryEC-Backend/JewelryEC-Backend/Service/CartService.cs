using JewelryEC_Backend.Data;
using JewelryEC_Backend.Models.CartItems.Dto;
using JewelryEC_Backend.Models.CartItems.Entities;
using JewelryEC_Backend.Models.Carts.Dto;
using JewelryEC_Backend.Models.Carts.Entities;
using JewelryEC_Backend.Service.IService;
using JewelryEC_Backend.UnitOfWork;

namespace JewelryEC_Backend.Service
{
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRedisShoppingCartService _cacheService;

        public CartService(IUnitOfWork unitOfWork, IRedisShoppingCartService cacheService)
        {
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
        }

        public CartItem CartUpSert(Guid userId, CartItem cartItem)
        {
            if (isUserExist(userId) && isProductExist(cartItem.ProductId))
            {
                var cartFromDb = _unitOfWork.Carts.GetCartHeader(userId);
                if (cartFromDb == null)
                {
                    //create cart new and details 
                    Cart cart = new Cart();
                    cart.Id = Guid.NewGuid();
                    cart.UserId = userId;
                    _unitOfWork.Carts.Add(cart);
                    _unitOfWork.Save();
                    // create additional for cart item
                    cartItem.CartId = cart.Id;
                    cartItem.Id = Guid.NewGuid();
                    _unitOfWork.CartItems.Add(cartItem);
                    _unitOfWork.Save();
                    // set data into cache
                    _cacheService.SetData(userId, cartItem.ProductId, cartItem.Count);
                }
                else
                {
                    //if header is not null
                    //check if details has same product
                    var cartDetailsFromDb = _unitOfWork.CartItems.GetCartItem(cartItem.ProductId, cartFromDb.Id);
                    if (cartDetailsFromDb == null)
                    {
                        //create cartdetails
                        cartItem.CartId = cartFromDb.Id;
                        cartItem.Id = Guid.NewGuid();
                        _unitOfWork.CartItems.Add(cartItem);
                        _unitOfWork.Save();
                        // set data into cache 
                        _cacheService.SetData(userId, cartItem.ProductId, cartItem.Count);
                    }
                    else
                    {
                        //update count in cart details
                        cartDetailsFromDb.Count += cartItem.Count;
                        _unitOfWork.CartItems.Update(cartDetailsFromDb);
                        _unitOfWork.Save();
                        // set data into cache 
                        _cacheService.SetData(userId, cartItem.ProductId, cartItem.Count);
                        cartItem.Count = cartDetailsFromDb.Count;

                    }

                }
                    return cartItem;
            }
            return null;

        }

        private bool isProductExist(Guid  productId)
        {
            var productFrmDB =   _unitOfWork.Products.GetById(productId);
            return productFrmDB == null ? false: true;
        }

        private bool isUserExist(Guid userId)
        {
            var userFrmDB = _unitOfWork.Users.GetById(userId);
            return userFrmDB == null ? false : true;
        }

        public bool CreateCart(Cart catalogToCreate)
        {
           try
           {
                _unitOfWork.Carts.Add(catalogToCreate);
                return true;
           }
            catch (Exception ex)
           {
                return false;
           }
        }

        public Cart GetDetailCart(Guid userId)
        {
 
            if(isUserExist(userId))
            {

                // check cache data
                var cacheData = _cacheService.GetData(userId);
                if (cacheData != null )
                {
                    Cart getCart = new Cart();
                    getCart.UserId = userId;  
                    getCart.cartItems = new List<CartItem>();
                    foreach (var item in cacheData)
                    {
                        // lấy thêm name, giá sau khi merge code 
                        getCart.cartItems.Add(new CartItem { ProductId = item.Key, Count = item.Value });
                    }

                    return getCart;
                }
                else
                {
                    var CartFrmDb = _unitOfWork.Carts.GetCartHeader(userId);
                    if (CartFrmDb == null)
                    {
                        return null;
                        //set null 
                    }
                    else
                    {

                        CartFrmDb.cartItems = _unitOfWork.CartItems.GetCartItems(CartFrmDb.Id).ToList();


                        // set CACHE
                        if (CartFrmDb.cartItems.Count() == 0)
                        {
                            // set null to cache 
                        }
                        else
                        {
                            foreach (var item in CartFrmDb.cartItems)
                            {
                                _cacheService.SetData(userId, item.ProductId, item.Count);
                            }
                        }
                    }
                    //var expiryTime = DateTimeOffset.Now.AddSeconds(30);
                    return CartFrmDb;
                }
              
            }
            return null;

        }

        public void SetStatusForCart(int status, Guid cartId)
        {
             _unitOfWork.Carts.SetStatusForCart(status, cartId);  
        }

        public bool DeleteCartItem(Guid userId, Guid productId)
        {
            try
            {
                // tìm cart của user
                var cartFrmDb = _unitOfWork.Carts.GetCartHeader(userId);
                if (cartFrmDb != null)
                {
                    // kt trong cart có product đó k
                    var cartItemFrmDb = _unitOfWork.CartItems.FindCartItem(cartFrmDb.Id, productId);
                    if (cartItemFrmDb != null)
                    {
                        // nếu có xóa trên db
                        _unitOfWork.CartItems.Remove(cartItemFrmDb);
                        _unitOfWork.Save();
                        // tiếp, xóa product id trên cache of user
                        _cacheService.RemoveProductFromCart(userId, productId);
                        return true;
                    }
                    return false;
                }
                else return false;
            }catch (Exception ex)
            {
                return false;
            }
           
              
        }
    }
}
