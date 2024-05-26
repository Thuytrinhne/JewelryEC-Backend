using JewelryEC_Backend.Data;
using JewelryEC_Backend.Models.CartItems.Dto;
using JewelryEC_Backend.Models.CartItems.Entities;
using JewelryEC_Backend.Models.Carts.Dto;
using JewelryEC_Backend.Models.Carts.Entities;
using JewelryEC_Backend.Models.OrderItems;
using JewelryEC_Backend.Models.Products;
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
            if (isUserExist(userId) && isProductExist(cartItem.ProductItemId))
            {
                var cartFromDb = _unitOfWork.Carts.GetCartHeader(userId);
                if (cartFromDb == null)
                {
                    if (cartItem.Count <= 0)
                        return null;    
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
                    _cacheService.SetData(userId, cartItem.ProductItemId, cartItem.Count);
                }
                else
                {
                    //if header is not null
                    //check if details has same product
                    var cartDetailsFromDb = _unitOfWork.CartItems.GetCartItem(cartItem.ProductItemId, cartFromDb.Id);
                    if (cartDetailsFromDb is null)
                    {
                        //create cartdetails
                        cartItem.CartId = cartFromDb.Id;
                        cartItem.Id = Guid.NewGuid();
                        _unitOfWork.CartItems.Add(cartItem);
                        _unitOfWork.Save();
                        // set data into cache 
                        _cacheService.SetData(userId, cartItem.ProductItemId, cartItem.Count);
                    }
                    else
                    {
                        //update count in cart details
                        if (cartDetailsFromDb.Count <= -cartItem.Count)
                            return null;
                            cartDetailsFromDb.Count += cartItem.Count;
                        _unitOfWork.CartItems.Update(cartDetailsFromDb);
                        _unitOfWork.Save();
                        // set data into cache 
                        _cacheService.SetData(userId, cartItem.ProductItemId, cartItem.Count);
                        cartItem.Count = cartDetailsFromDb.Count;

                    }

                }
                return cartItem;
            }
            return null;

        }

        private bool isProductExist(Guid productId)
        {
            var productFrmDB = _unitOfWork.ProductItem.GetById(productId);
            return productFrmDB == null ? false : true;
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
                // check cache data
                var cacheData = _cacheService.GetData(userId);
                if (cacheData != null)
                {
                    Cart getCart = new Cart();
                    getCart.UserId = userId;
                    getCart.Items = new List<CartItem>();
                    foreach (var item in cacheData)
                    {
                        getCart.Items.Add(new CartItem { ProductItemId = item.Key, Count = item.Value });
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

                        CartFrmDb.Items = _unitOfWork.CartItems.GetCartItems(CartFrmDb.Id).ToList();
                        // set CACHE
                        // Gọi phương thức SetCartTTL với thời gian sống là 15 phút
                        TimeSpan expiry = TimeSpan.FromMinutes(15);
                        _cacheService.SetCartTTL(userId, expiry);
                        if (CartFrmDb.Items.Count() == 0)
                        {
                            _cacheService.SetCartHeaderNul(userId);
                        }
                        else
                        {
                            foreach (var item in CartFrmDb.Items)
                            {
                                _cacheService.SetData(userId, item.ProductItemId, item.Count);
                            }
                        }
                    }
                    var expiryTime = DateTimeOffset.Now.AddMinutes(15);
                    return CartFrmDb;
                }

            
            return null;

        }

        

        public bool DeleteCartItem(Guid userId, Guid productId)
        {
            try
            {
                if (!isProductExist(productId)) { return false; }
                // tìm cart của user
                var cartFrmDb = _unitOfWork.Carts.GetCartHeader(userId);
                if (cartFrmDb is not null)
                {

                    // kt trong cart có product đó k
                    var cartItemFrmDb = _unitOfWork.CartItems.FindCartItem(cartFrmDb.Id, productId);
                    if (cartItemFrmDb != null)
                    {
                        // nếu có xóa trên db
                        _unitOfWork.CartItems.Remove(cartItemFrmDb);
                        _unitOfWork.Save();

                        // nếu đó là sp cuối cùng thì xóa cart luôn
                        var count = _unitOfWork.CartItems.GetAll(u => u.CartId == cartFrmDb.Id).Count;
                       if (count == 0)
                        {
                            _unitOfWork.Carts.Remove(cartFrmDb); _unitOfWork.Save();
                            _cacheService.RemoveCartHeader(userId);
                        }
                       else
                        // tiếp, xóa product id trên cache of user
                        _cacheService.RemoveProductFromCart(userId, productId);

                        return true;
                    }
                    return false;
                }
                 return false;
            }
            catch (Exception ex)
            {
                return false;
            }


        }
        public ProductVariant GetCartItemDetail(Guid productId)
        {
            return _unitOfWork.ProductItem.GetById(productId);
        }
        public string GetCartItemName(Guid productId)
        {
            return _unitOfWork.Products.GetById(productId).Name;
        }
        public void HanldeCartAfterCheckout(Guid userId, List<OrderItem> orderItems)
        {
            var cart = _unitOfWork.Carts.GetCartHeader(userId);
            foreach (var item in orderItems)
            {
                // delete items from cart details
           
                var cartItemsToDelete = _unitOfWork.CartItems.Find(i=>i.CartId == cart.Id && i.ProductItemId == item.ProductItemId).FirstOrDefault();
                _unitOfWork.CartItems.Remove(cartItemsToDelete);
                _unitOfWork.Save();

                // delete cart from cache redis
                _cacheService.RemoveProductFromCart(userId, item.ProductItemId);
            }
            // delete cart header if not exist cartitem
            var cartHeaderItems = _unitOfWork.CartItems.GetCartItems(cart.Id).Count();
            if (cartHeaderItems == 0)
            {
                _unitOfWork.Carts.Delete(cart);
                _unitOfWork.Save();
                _cacheService.RemoveCartHeader(userId);
            }
        }

        
    }
}
