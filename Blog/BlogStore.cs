using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Photoblog.Api.Blog
{
    public class BlogStore
    {
        public const string AllPostsCacheKey = "AllPosts";
        public const string AllCategoriesCacheKey = "AllCategories";

        private BlogDbContext context;
        private IMemoryCache memoryCache;

        public BlogStore(IMemoryCache memoryCache, BlogDbContext context)
        {
            this.memoryCache = memoryCache;
            this.context = context;
        }

        public List<Post> GetAllPosts(bool hideFutureDates = true)
        {
            if (!memoryCache.TryGetValue(AllPostsCacheKey, out List<Post> allPosts))
            {
                allPosts = context
                    .Posts
                    .Include(x => x.Category)
                    .Include(x => x.Images)
                    .OrderByDescending(x => x.Date)
                    .ToList();

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromDays(365));

                // Save data in cache.
                memoryCache.Set(AllPostsCacheKey, allPosts, cacheEntryOptions);
            }

            if (hideFutureDates)
            {
                allPosts = RemoveFuturePosts(allPosts);
            }

            return allPosts;
        }

        private List<Post> RemoveFuturePosts(ICollection<Post> posts)
        {
            return posts.Where(x => x.Date <= DateTime.Today).ToList();
        }

        public Post GetPostByLink(string link, bool hideFutureDates = true)
        {
            var allPosts = GetAllPosts(hideFutureDates);

            return allPosts.FirstOrDefault(x => x.Link == link);
        }

        public Post GetPost(int id, bool hideFutureDates = true)
        {
            var allPosts = GetAllPosts(hideFutureDates);

            return allPosts.FirstOrDefault(x => x.Id == id);
        }

        public List<Category> GetAllCategories(bool hideFutureDates = true)
        {
            if (!memoryCache.TryGetValue(AllCategoriesCacheKey, out List<Category> allCategories))
            {
                allCategories = context
                    .Categories
                    .Include(x => x.Posts)
                        .ThenInclude(x => x.Images)
                    .OrderBy(x => x.Name)
                    .ToList();

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromDays(365));

                // Save data in cache.
                memoryCache.Set(AllCategoriesCacheKey, allCategories, cacheEntryOptions);
            }

            if (hideFutureDates)
            {
                foreach (var category in allCategories)
                {
                    category.Posts = RemoveFuturePosts(category.Posts);
                }
            }

            return allCategories;
        }

        public Category GetCategory(int id, bool hideFutureDates = true)
        {
            var allCategories = GetAllCategories(hideFutureDates);

            return allCategories.FirstOrDefault(x => x.Id == id);
        }

        public List<Image> GetImagesByPostId(int postId, bool hideFutureDates = true)
        {
            var images = GetPost(postId, hideFutureDates).Images;

            return images == null ? null : images.ToList();
        }

        public Image GetImage(int id)
        {
            return context.Images.FirstOrDefault(x => x.Id == id);
        }

        public void Create<TEntity>(TEntity entity)
            where TEntity : class
        {
            context.Add(entity);

            context.SaveChanges();

            ClearCache();
        }

        public void Update<TEntity>(TEntity entity)
            where TEntity : class
        {
            context.Update(entity);

            context.SaveChanges();

            ClearCache();
        }

        public void Delete<TEntity>(TEntity entity)
            where TEntity : class
        {
            context.Remove(entity);

            context.SaveChanges();

            ClearCache();
        }

        private void ClearCache()
        {
            memoryCache.Remove(AllCategoriesCacheKey);
            memoryCache.Remove(AllPostsCacheKey);
        }
    }
}
