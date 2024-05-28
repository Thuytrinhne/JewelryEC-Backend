﻿using JewelryEC_Backend.Enum;
using JewelryEC_Backend.Models.Addresses;
using JewelryEC_Backend.Models.Deliveries;
using JewelryEC_Backend.Models.Deliveries.Dto;
using JewelryEC_Backend.Models.OrderItems;
using JewelryEC_Backend.Models.Orders.Dto;
using JewelryEC_Backend.Models.Shippings;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace JewelryEC_Backend.Mapper
{
    public class ShippingMapper
    {
        public static Shipping ShippingFromCreateNewOrderDto(Guid? deliveryId, CreateDeliveryDto? createDeliveryDto, Guid userId)
        {
            Shipping shipping = new Shipping
            {
                Id = new Guid(),
                expectationShippingDate = DateTime.Today.AddDays(2),
                actualShippingDate = DateTime.Today.AddDays(3),
                ShippingStatus = ShippingStatus.Pending,
            };
            if (deliveryId == null)
            {
                Delivery delivery = new Delivery
                {
                    UserId = userId,
                    IsDepartment = createDeliveryDto.IsDepartment,
                    ReceiverIsMe = createDeliveryDto.ReceiverIsMe,
                    Information = createDeliveryDto.Information,
                    Address = new Address
                    {
                        Province = createDeliveryDto.Province,
                        District = createDeliveryDto.District,
                        Ward = createDeliveryDto.Ward,
                    }
                };
                shipping.Delivery = delivery;
            }
            else shipping.DeliveryId = (Guid)deliveryId;
            return shipping;
        }
    }
}
