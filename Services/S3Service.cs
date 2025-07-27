using Web_Manage.Models;
using Microsoft.EntityFrameworkCore;
using Amazon.S3.Transfer;
using Amazon.S3;
using Amazon.S3.Model;

namespace Web_Manage.Services
{
    public class S3Service
    {
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName = "cf-templates-29w3zrvbs88r-us-east-1";
        private readonly ApplicationDbContext _context;

        public S3Service(IAmazonS3 s3Client, ApplicationDbContext context)
        {
            _s3Client = s3Client;
            _context = context;
        }
        public async Task<Image> GetImagesByIdAsync(int id)
        {
            return await _context.images
                .Where(img => img.IdDonHang == id || img.IdLanDatHang == id).FirstOrDefaultAsync();
        }

        public async Task<Image?> AddImageAsync(Image image)
        {
            _context.images.Add(image);
            await _context.SaveChangesAsync();
            return image;
        }

        public async Task DeleteImageAsync(string fileKey)
        {
            var image = await _context.images.FindAsync(fileKey);
            if (image != null)
            {
                _context.images.Remove(image);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File không hợp lệ");

            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);

            using (var stream = file.OpenReadStream())
            {
                var uploadRequest = new TransferUtilityUploadRequest
                {
                    InputStream = stream,
                    Key = fileName,
                    BucketName = _bucketName,
                    ContentType = file.ContentType,
                };

                var transferUtility = new TransferUtility(_s3Client);
                await transferUtility.UploadAsync(uploadRequest);
            }

            return fileName;
        }

        public string GeneratePresignedUrl(string key)
        {
            var request = new GetPreSignedUrlRequest
            {
                BucketName = _bucketName,
                Key = key,
                Expires = DateTime.UtcNow.AddMinutes(15)
            };
            return _s3Client.GetPreSignedURL(request);
        }


        public string GetFileUrl(string key)
        {
            return $"https://{_bucketName}.s3.amazonaws.com/{key}";
        }
    }
}
