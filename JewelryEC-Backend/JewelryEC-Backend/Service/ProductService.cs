using AutoMapper;
using JewelryEC_Backend.Core.Filter;
using JewelryEC_Backend.Core.Utilities.Results;
using JewelryEC_Backend.Mapper;
using JewelryEC_Backend.Models;
using JewelryEC_Backend.Models.Products;
using JewelryEC_Backend.Models.Products.Dto;
using JewelryEC_Backend.Repository.IRepository;
using JewelryEC_Backend.Service.IService;
using JewelryEC_Backend.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Protocol;
using System.Drawing.Printing;
using System.Dynamic;

namespace JewelryEC_Backend.Service
{
    public class ProductService: IProductService
    {
        private IProductRepository _productDal;
        private IProductItemRespository _productItemRespository;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _productDal = unitOfWork.Products;
            _productItemRespository = unitOfWork.ProductItem;
        }
        public async Task<ResponseDto> GetAll(int pageNumber, int pageSize)
        {
            try
            {
                return new SuccessDataResult<List<Product>>(await _productDal.GetProducts(pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                return new ErrorResult(ex.Message);
            }
        }
        public async Task<ResponseDto> Add(CreateProductDto productDto)
        {
            Product product = ProductMapper.ProductFromCreateProductDto(productDto);
            Console.WriteLine(product.ToJson());
            await _productDal.AddAsync(product);
            await _productDal.SaveChangeAsync();
            return await this.GetById(product.Id);
        }
        public async Task<ResponseDto> Get(RootFilter? filters, int pageNumber, int pageSize)
        {
            var productList = new List<Product>();
            long totalCount = 0;
            if (filters != null)
            {
                totalCount = await _productDal.GetTotalCount(CompositeFilter<Product>.ApplyFilter(filters));
                productList = await _productDal.GetProducts(pageNumber, pageSize, CompositeFilter<Product>.ApplyFilter(filters));
            }
            else
            {
                totalCount = await _productDal.GetTotalCount(null);
                productList = await _productDal.GetProducts(pageNumber, pageSize);
            }
            var response = new
            {
                Data = productList,
                TotalCount = totalCount
            };
            return new SuccessDataResult<object>(response);
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
            await this.deleteProductItemsFromProduct(updateProduct.Id);
            Console.Write(updateProduct);
            await _productDal.Update(updateProduct);
            return await this.GetById(updateProduct.Id);
        }
        private async Task deleteProductItemsFromProduct(Guid productId)
        {
            Product product = await _productDal.GetProduct(Product => Product.Id == (productId));
            if(product != null && product.Items.Count > 0)
            {
                _productItemRespository.RemoveRange(product.Items);
            }
            _productDal.Detach(product);
        }

        public async Task<ResponseDto> GetById(Guid id)
        {
            Product product = await _productDal.GetProduct(Product => Product.Id == (id));
            if (product != null)
            {
                return new SuccessDataResult<Product>(product);
            }
            return new ErrorResult();
        }
    }
}
