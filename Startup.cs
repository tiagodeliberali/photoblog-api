using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Photoblog.Api.Admin;
using Photoblog.Api.Blog;

namespace Photoblog.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<BlogDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Blog")));
            services.AddTransient<BlogStore, BlogStore>();

            services.Configure<BlogSettings>(Configuration.GetSection("BlogSettings"));

            services.AddMemoryCache();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddCors(options =>
                options.AddPolicy("CorsPolicy", builder => {
                    builder
                        .WithOrigins("http://tiagophotoblog.com.br", "https://tiagophotoblog.com.br")
                        .AllowAnyMethod()
                        .AllowCredentials()
                        .AllowAnyHeader();
                }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseCors("CorsPolicy");
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "admin_pages",
                    template: "admin/{controller=Posts}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "metadata",
                    template: "metadata/{link?}",
                    defaults: new { controller = "Home", action = "Metadata" });

                routes.MapRoute(
                    name: "default",
                    template: "{*.}",
                    defaults: new { controller = "Home", action = "Index" });
            });
        }
    }
}
