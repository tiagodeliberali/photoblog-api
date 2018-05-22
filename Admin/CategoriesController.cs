using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Photoblog.Api.Blog;
using Photoblog.Controllers.Admin;

namespace Photoblog.Api.Admin
{
    //[Authorize]
    public class CategoriesController : AdminBaseController
    {
        public CategoriesController(BlogStore blogStore) : base(blogStore)
        { }

        public IActionResult Index()
        {
            return View(blogStore.GetAllCategories(false));
        }

        public IActionResult Details(int id)
        {
            return GetCategory(id);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Description,Name")] Category category)
        {
            return CreateEntity(category, 
                () => RedirectToAction("Index"));
        }

        public IActionResult Edit(int id)
        {
            return GetCategory(id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Description,Name")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            return UpdateEntity(category, 
                () => RedirectToAction("Index"));
        }

        public IActionResult Delete(int id)
        {
            return GetCategory(id);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var category = blogStore.GetCategory(id, false);

            blogStore.Delete(category);

            return RedirectToAction("Index");
        }

        private IActionResult GetCategory(int id)
        {
            var category = blogStore.GetCategory(id, false);

            return NotNullView(category);
        }
    }
}
