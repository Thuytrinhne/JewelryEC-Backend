

using JewelryEC_Backend.Models;

namespace JewelryEC_Backend.Service.Generics
{
    public interface IGenericCrudOperationService<T>
    {
        Task<ResponseDto> GetAll();
    }
}
