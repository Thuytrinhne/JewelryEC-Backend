
using JewelryEC_Backend.Helpers.Payments.VnPay;

namespace JewelryEC_Backend.Service.IService
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(HttpContext context, VnPaymentRequestModel model);
        VnPaymentResponseModel PaymentExecute(IQueryCollection collections);
    }
}
