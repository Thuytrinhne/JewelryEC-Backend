

using JewelryEC_Backend.Models;

namespace JewelryEC_Backend.Service.Generics
{
    public interface IGenericCrudOperationService<T>
    {
        Task<ResponseDto> GetAll();
        Task<ResponseDto> Add(T entity);
        Task<ResponseDto> Delete(T entity);
        Task<ResponseDto> Update(T entity);
    }
}
