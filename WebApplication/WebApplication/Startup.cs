using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using WebApplication.Database;
using WebApplication.Models;

namespace WebApplication
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<MyDbContext>(builder =>
            {
                builder.UseSqlServer();
            });
            services.AddRouting();
            services.AddControllers().AddOData(opt => opt.AddRouteComponents(CreateModel()).Expand().Filter().Count().Select().OrderBy().SetMaxTop(1000));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()?.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<MyDbContext>();
                context.Database.Migrate();

                if (!context.Parents.Any())
                {
                    SeedInitialData(context);
                }
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseODataRouteDebug();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static void SeedInitialData(MyDbContext context)
        {
            var c1 = new Child
            {
                Name = "c1",
                InternalId = new Guid("10000000-0000-0000-0000-000000000000")
            };

            var c2 = new Child
            {
                Name = "c2",
                InternalId = new Guid("20000000-0000-0000-0000-000000000000")
            };

            var p1 = new Parent
            {
                Name = "p1",
                Children = new List<Child>
                {
                    c1,
                    c2
                },
                InternalId = new Guid("30000000-0000-0000-0000-000000000000")
            };

            context.Children.Add(c1);
            context.Children.Add(c2);

            context.Parents.Add(p1);

            context.SaveChanges();
        }

        private static IEdmModel CreateModel()
        {
            var builder = new ODataConventionModelBuilder();

            var parentsSet = builder.EntitySet<Parent>("Parents");
            var childrenSet = builder.EntitySet<Child>("Children");

            parentsSet.EntityType.HasKey(e => e.InternalId);
            parentsSet.EntityType.Property(e => e.InternalId).Name = "Id";
            parentsSet.EntityType.Ignore(e => e.Id);

            childrenSet.EntityType.HasKey(e => e.InternalId);
            childrenSet.EntityType.Property(e => e.InternalId).Name = "Id";
            childrenSet.EntityType.Ignore(e => e.Id);

            return builder.GetEdmModel();
        }
    }
}