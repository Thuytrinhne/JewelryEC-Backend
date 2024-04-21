using JewelryEC_Backend.Core.Repository.EntityFramework;
using JewelryEC_Backend.Data;
using JewelryEC_Backend.Models.Auths.Entities;
using JewelryEC_Backend.Repository.IRepository;

namespace JewelryEC_Backend.Repository
{
    public class EmailVerificationRespository : GenericRepository<EmailVerification>, IEmailVerificationRespository
    {
        public EmailVerificationRespository(AppDbContext context) : base(context)
        {
        }

        public EmailVerification GetEntityByEmail(string email)
        {
            var entity =  Find(x => x.Email == email)
                    .OrderByDescending(x => x.Created_at)
                    .FirstOrDefault();
            return entity == null ? null : entity;
        }

       
    }
}
