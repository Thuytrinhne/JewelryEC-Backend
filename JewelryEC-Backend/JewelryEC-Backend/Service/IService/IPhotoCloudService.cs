
using CloudinaryDotNet.Actions;

namespace JewelryEC_Backend.Service.IService
{
    public interface IPhotoCloudService
    {
        Task<ImageUploadResult> AddPhotoAsync(IFormFile file);
        Task<DeletionResult> DeletePhotoAsync(string publicId);

    }
}
