using AutoMapper;
using JewelryEC_Backend.Core.Utilities.Results;
using JewelryEC_Backend.Mapper;
using JewelryEC_Backend.Models;
using JewelryEC_Backend.Models.Products;
using JewelryEC_Backend.Models.Products.Dto;
using JewelryEC_Backend.Repository.IRepository;
using JewelryEC_Backend.Service.IService;
using JewelryEC_Backend.UnitOfWork;
using NuGet.Protocol;

namespace JewelryEC_Backend.Service
{
    public class ProductService: IProductService
    {
        private IProductRepository _productDal;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _productDal = unitOfWork.Products;
        }
        public async Task<ResponseDto> GetAll()
        {
            return new SuccessDataResult<List<Product>>(await _productDal.GetAllAsync());
        }
        public async Task<ResponseDto> Add(CreateProductDto productDto)
        {
            Product product = ProductMapper.ProductFromCreateProductDto(productDto);
            Console.WriteLine(product.ToJson());
            await _productDal.AddAsync(product);
            await _productDal.SaveChangeAsync();
            return new SuccessResult("Add product successfully");
        }
        public async Task<ResponseDto> MultiAdd(CreateProductDto[] productDtos)
        {
            List<Product> products = productDtos.Select(productDto => ProductMapper.ProductFromCreateProductDto(productDto)).ToList();
            await _productDal.MultiAddAsync(products.ToArray());
            return new SuccessResult();
        }

        public async Task<ResponseDto> Delete(Guid productId)
        {
            Product product = await _productDal.GetProduct(Product => Product.Id == (productId));
            if(product != null)
            {
                await _productDal.Delete(product);
                return new SuccessResult();
            }
            return new ErrorResult();
        }
        public async Task<ResponseDto> Update( UpdateProductDto productDto )
        {
            Product updateProduct = ProductMapper.ProductFromUpdateProductDto(productDto);
            Console.Write(updateProduct);
            _productDal.Update(updateProduct);
            return new SuccessResult();
        }

        public async Task<ResponseDto> GetById(Guid id)
        {
            Product product = await _productDal.GetProduct(Product => Product.Id == (id));
            if (product != null)
            {
                //await _productDal.Delete(product);
                //return new SuccessResult();
                return new SuccessResult(product);
            }
            return new ErrorResult();
        }
    }
}
