using CloudinaryDotNet.Actions;

namespace JewelryEC_Backend.Service.IService
{
    public interface IPhotoService
    {
        void AddPhotoAsync(Guid UserId, string AvatarUrl, string PublicId);
        void DeletePhotoAsync(Guid UserId);
    }
}
