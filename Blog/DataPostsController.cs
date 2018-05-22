using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Photoblog.Api.Blog
{
    [Route("api/[controller]")]
    public class DataPostsController
    {
        private BlogStore blogStore;

        public DataPostsController(BlogStore blogStore)
        {
            this.blogStore = blogStore;
        }

        [HttpGet]
        public List<Post> Get()
        {
            return blogStore.GetAllPosts();
        }

        [HttpGet("{id}")]
        public Post Get(string id)
        {
            return blogStore.GetPostByLink(id);
        }
    }
}
