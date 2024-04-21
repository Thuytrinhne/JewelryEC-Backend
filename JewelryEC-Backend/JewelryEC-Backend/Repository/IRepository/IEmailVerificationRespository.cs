using JewelryEC_Backend.Core.Repository;
using JewelryEC_Backend.Models.Auths.Entities;

namespace JewelryEC_Backend.Repository.IRepository
{
    public interface IEmailVerificationRespository : IGenericRepository<EmailVerification>
    {
        EmailVerification GetEntityByEmail (string email);   
    }
}
