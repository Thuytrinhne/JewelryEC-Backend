using JewelryEC_Backend.Service.IService;
using JewelryEC_Backend.UnitOfWork;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace JewelryEC_Backend.Service
{
    public class PhotoService(IUnitOfWork _unitOfWork) : IPhotoService
    {
        public  void AddPhotoAsync(Guid UserId, string AvatarUrl, string PublicId)
        {
            var user = _unitOfWork.Users.GetById(UserId);
            user.AvatarUrl = AvatarUrl;
            user.PublicId = PublicId;
            _unitOfWork.Save();
        }

        public void DeletePhotoAsync(Guid UserId)
        {
            var user = _unitOfWork.Users.GetById(UserId);
            user.AvatarUrl = string.Empty;
            user.PublicId = string.Empty;
            _unitOfWork.Save();
        }
    }
}
