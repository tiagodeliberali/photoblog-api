using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Photoblog.Api.Blog
{
    [Route("api/[controller]")]
    public class DataCategoriesController
    {
        private BlogStore blogStore;

        public DataCategoriesController(BlogStore blogStore)
        {
            this.blogStore = blogStore;
        }

        [HttpGet]
        public List<Category> Get()
        {
            return blogStore.GetAllCategories();
        }

        [HttpGet("{id}")]
        public Category Get(int id)
        {
            return blogStore.GetCategory(id);
        }
    }
}
