using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Photoblog.Api.Admin;

namespace Photoblog.Api.Blog
{
    public class HomeController : Controller
    {
        private BlogSettings blogSettings;
        private IConfiguration configuration;

        public HomeController(IOptions<BlogSettings> blogSettings, IConfiguration configuration)
        {
            this.blogSettings = blogSettings.Value;
            this.configuration = configuration;
        }

        public IActionResult Index(int postId)
        {
            ViewBag.ConnectionString = configuration.GetConnectionString("Blog");
            return View(blogSettings);
        }
    }
}