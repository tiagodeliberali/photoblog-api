namespace Photoblog.Api.Blog
{
    public class Image
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public int PostId { get; set; }
    }
}
