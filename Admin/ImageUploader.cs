using Photoblog.Api.Blog;

namespace Photoblog.Api.Admin
{
    public class ImageUploader
    {
        public Image Image { get; set; }
        public string ApiKey { get; set; }
        public string UploadPreset { get; set; }
    }
}
