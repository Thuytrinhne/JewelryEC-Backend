using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using JewelryEC_Backend.Service.IService;
using JewelryEC_Backend.Utility;
using Microsoft.Extensions.Options;

namespace JewelryEC_Backend.Service
{
    public class PhotoCloudService
        : IPhotoCloudService
    {
        private readonly Cloudinary _cloudinary;
        public PhotoCloudService(IOptions<CloudinarySettingsKey> config) {
            var account = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
                );
            _cloudinary = new Cloudinary( account );    
        }
        public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
        {
            var uploadResult = new ImageUploadResult();
            if(file.Length >0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.Name, stream),
                    Transformation = new Transformation()
                    .Height(500).Width(500).Crop("fill").Gravity("face")
                };
                uploadResult = await _cloudinary.UploadAsync(uploadParams);
            }
            return uploadResult;

        }

        public async Task<DeletionResult> DeletePhotoAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deleteParams);
            return result;

         }
    }
}
