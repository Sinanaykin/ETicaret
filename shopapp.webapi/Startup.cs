using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using shopapp.business.Abstract;
using shopapp.business.Concrete;
using shopapp.data.Abstract;
using shopapp.data.Concrete.EfCore;

namespace shopapp.webapi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        readonly string MyAllowOrigins="_myAllowOrigins"; //MyAllowOrigins i tanımlıyoruz
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContext<ShopContext>(options=>options.UseSqlServer(Configuration.GetConnectionString("MsSqlConnection")));
           
            services.AddScoped<IUnitOfWork,UnitOfWork>(); 

            services.AddScoped<ICategoryService,CategoryManager>();        
            services.AddScoped<IProductService,ProductManager>();
            services.AddScoped<ICartService,CartManager>();
            services.AddScoped<IOrderService,OrderManager>();

            services.AddControllers();


           

            services.AddCors(options => { 
                options.AddPolicy( //yeni bir tane sözleşme ekliyoruz
                    name:MyAllowOrigins, //isim veriyoruz policy a
                    builder => {
                        builder
                        .AllowAnyOrigin() //bütün taleplere izin verir.Kısıtlanıdradabiliriz url ekleyerek.WithOrigin("http://127.0.0.1:5501/index.html") bu şekilde sadece bu adrese izin verir
                        .AllowAnyHeader()//belli parametrelere izin verir
                        .AllowAnyMethod();//get post gibi metodlara izin verir
                    }
                );
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

           // app.UseHttpsRedirection();

            app.UseRouting();
            
            app.UseCors(MyAllowOrigins);//Yukarıda  hazırladığımızı kullanmalıyız burda

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
