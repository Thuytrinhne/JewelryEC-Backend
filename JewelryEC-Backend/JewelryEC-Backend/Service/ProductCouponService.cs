using AutoMapper;
using JewelryEC_Backend.Core.Utilities.Results;
using JewelryEC_Backend.Mapper;
using JewelryEC_Backend.Models;
using JewelryEC_Backend.Models.Coupon;
using JewelryEC_Backend.Models.Coupon.Dto;
using JewelryEC_Backend.Models.Coupon.Dto;
using JewelryEC_Backend.Repository.IRepository;
using JewelryEC_Backend.Service.IService;
using JewelryEC_Backend.UnitOfWork;
using NuGet.Protocol;

namespace JewelryEC_Backend.Service
{
    public class ProductCouponService : IProductCouponService
    {
        private IProductCouponRepository _productCouponDal;

        private IMapper _mapper;

        public ProductCouponService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _productCouponDal = unitOfWork.ProductCoupons;
            _mapper = mapper;
        }
        public async Task<ResponseDto> GetAll()
        {
            return new SuccessDataResult<List<ProductCoupon>>(await _productCouponDal.GetAllAsync());
        }
        public async Task<ResponseDto> Add(CreateProductCouponDto productCouponDto)
        {
            ProductCoupon productCoupon = _mapper.Map<ProductCoupon>(productCouponDto);
            //if (!validateProductCoupon(productCoupon))
            //{
            //    return new ErrorResult("Invalid productCoupon");
            //}
            Console.WriteLine(productCoupon.ToJson());
            await _productCouponDal.AddAsync(productCoupon);
            await _productCouponDal.SaveChangeAsync();
            return new SuccessResult("Add productCoupon successfully", _productCouponDal.Get(p => p.Id == productCoupon.Id));
        }
        public async Task<ResponseDto> MultiAdd(CreateProductCouponDto[] productCouponDtos)
        {
            List<ProductCoupon> productCoupons = productCouponDtos.Select(productCouponDto => _mapper.Map<ProductCoupon>(productCouponDto)).ToList();
            await _productCouponDal.MultiAddAsync(productCoupons.ToArray());
            return new SuccessResult();
        }

        public async Task<ResponseDto> GetById(Guid id)
        {
            ProductCoupon productCoupon = _productCouponDal.Get(ProductCoupon => ProductCoupon.Id == id);
            return new SuccessDataResult<ProductCoupon>(productCoupon);
        }

        public async Task<ResponseDto> Update(UpdateProductCouponDto couponDto)
        {
            ProductCoupon productCoupon = _mapper.Map<ProductCoupon>(couponDto);
            await _productCouponDal.Update(productCoupon);
            await _productCouponDal.SaveChangeAsync();
            return await this.GetById(productCoupon.Id);
        }
        public async Task<ResponseDto> Delete(Guid id)
        {
            ProductCoupon productCoupon = _productCouponDal.Get(ProductCoupon => ProductCoupon.Id == id);
            if (productCoupon != null)
            {
                await _productCouponDal.Delete(productCoupon);
                return new SuccessResult();
            }
            return new ErrorResult();
        }
        private Boolean validateProductCoupon(ProductCoupon productCoupon)
        {
            ProductCoupon existProductCoupon = _productCouponDal.GetAll(
                ProductCoupon =>  ProductCoupon.ProductId == productCoupon.ProductId && ProductCoupon.ValidTo > productCoupon.ValidFrom
                ).FirstOrDefault();
            Console.WriteLine(existProductCoupon.ToJson());
            if (productCoupon.ValidFrom < new DateTime() && productCoupon.ValidFrom > productCoupon.ValidTo || existProductCoupon != null)
            {
                return false;
            }
            return true;
        }
    }
}
