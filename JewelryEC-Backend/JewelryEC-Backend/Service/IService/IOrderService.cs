﻿using JewelryEC_Backend.Enum;
using JewelryEC_Backend.Models;
using JewelryEC_Backend.Models.Orders.Dto;
using JewelryEC_Backend.Models.Products;
using JewelryEC_Backend.Models.Products.Dto;

namespace JewelryEC_Backend.Service.IService
{
    public interface IOrderService
    {
        Task<ResponseDto> GetAll();
        Task<ResponseDto> GetById(Guid id);
        Task<ResponseDto> Add(CreateNewOrderDto orderDto);
        Task<ResponseDto> UpdateOrderStatus(Guid order, OrderStatus orderStatus);
        Task<ResponseDto> GetOrdersByUserId(Guid userId);
    }
}
