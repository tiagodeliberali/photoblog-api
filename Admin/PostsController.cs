using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Photoblog.Api.Blog;
using Photoblog.Controllers.Admin;

namespace Photoblog.Api.Admin
{
    //[Authorize]
    public class PostsController : AdminBaseController
    {
        public PostsController(BlogStore blogStore) : base(blogStore)
        { }

        public IActionResult Index()
        {
            return View(blogStore.GetAllPosts(false));
        }

        public IActionResult Details(int id)
        {
            return GetPost(id);
        }

        public IActionResult Create()
        {
            LoadCategorySelectList();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,CategoryId,Date,Description,Link,Title")] Post post)
        {
            LoadCategorySelectList(post.CategoryId);

            return CreateEntity(post,
                () => RedirectToAction("Create", "Images", new { postId = post.Id }));
        }

        public IActionResult Edit(int id)
        {
            var post = blogStore.GetPost(id, false);

            LoadCategorySelectList(post.CategoryId);

            return NotNullView(post);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,CategoryId,Date,Description,Link,Title")] Post post)
        {
            if (id != post.Id)
            {
                return NotFound();
            }

            LoadCategorySelectList(post.CategoryId);

            return UpdateEntity(post,
                () => RedirectToAction("Index"));
        }

        public IActionResult Delete(int id)
        {
            return GetPost(id);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var post = blogStore.GetPost(id, false);

            blogStore.Delete(post);

            return RedirectToAction("Index");
        }

        private IActionResult GetPost(int id)
        {
            var post = blogStore.GetPost(id, false);

            return NotNullView(post);
        }

        private void LoadCategorySelectList(int categoryId = 0)
        {
            ViewData["CategoryId"] = new SelectList(blogStore.GetAllCategories(false), "Id", "Name", categoryId);
        }
    }
}
