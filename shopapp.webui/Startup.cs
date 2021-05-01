using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using shopapp.webui.EmailServices;
using Microsoft.Extensions.FileProviders;
using shopapp.data.Abstract;
using shopapp.data.Concrete.EfCore;
using shopapp.business.Abstract;
using shopapp.webui.Identity;
using Microsoft.Extensions.Configuration;
using shopapp.business.Concrete;
using System.IO;

namespace shopapp.webui
{
    public class Startup
    {
       private IConfiguration _configuration;

       public Startup(IConfiguration configuration)
       {
           _configuration=configuration;
       }


        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
           //ShopContext içinde connecionstring bağlaması yapıyoruz. ConnectionStringi  ApplicationContext e de ekliyoruz alttaki.
         //   services.AddDbContext<ShopContext>(options=>options.UseSqlite(_configuration.GetConnectionString("SqliteConnection")));
          services.AddDbContext<ShopContext>(options=>options.UseSqlServer(_configuration.GetConnectionString("MsSqlConnection")));



           //adddbcontexte  applicationcontext i vericez.Zaten shopcontextimiz dbcontextden türüyo ,dbcontext imiz içinede applicationcontexti verdik bu sayede veritabnında user,role ve kendi oluşturduğumuz product, category vs. gibi tablolar oluşur migration yapınca
            // services.AddDbContext<ApplicationContext>(options=>options.UseSqlite(_configuration.GetConnectionString("SqliteConnection")));
             services.AddDbContext<ApplicationContext>(options=>options.UseSqlServer(_configuration.GetConnectionString("MsSqlConnection")));//MsSql e cevirdik Sqlite dan
            //Identity i tanımlamamız lazım programa.Identity ye kullanıcı bilgisi ve rol  tabloları için olan sınıfı kullanıcaz.
            //Eğer User classı oluşturmasaydık extra bilgiler için altta User yerine İdentityUser kullanıcaz
            //addentityframeworkstores içine kullanıcağımız contexti yaz.adddefaulttokenprovidersı da parola resetlemek için gerekli olan  benzersiz sayıyı üreticek yapı dır
   
            services.AddIdentity<User,IdentityRole>().AddEntityFrameworkStores<ApplicationContext>().AddDefaultTokenProviders();
            
            services.Configure<IdentityOptions>(options=>{
               //password
               options.Password.RequireDigit=true;
               options.Password.RequireLowercase=true;
               options.Password.RequireUppercase=true;
               options.Password.RequiredLength=6;
               options.Password.RequireNonAlphanumeric=true;
               
               //LocOut
               options.Lockout.MaxFailedAccessAttempts=5;
               options.Lockout.DefaultLockoutTimeSpan=TimeSpan.FromMinutes(5);
               options.Lockout.AllowedForNewUsers=true;

               //options.User.AllowedUserNameCharacters=""
               options.User.RequireUniqueEmail=true;
               options.SignIn.RequireConfirmedEmail=true;
               options.SignIn.RequireConfirmedPhoneNumber=false;
            });

            services.ConfigureApplicationCookie(options=>{

               options.LoginPath="/account/login";
               options.LogoutPath="/account/logout";
               options.AccessDeniedPath="/account/accessdenied";
               options.SlidingExpiration=true;
               options.ExpireTimeSpan=TimeSpan.FromMinutes(60);
               options.Cookie=new CookieBuilder //kullanıcı giriş yapınca her sayfa da tekrar tekrr giriş yapmaması için onu hatırlaması için kullanılır cookie ler.
               {
                  HttpOnly=true,
                  Name=".ShopApp.Security.Cookie",
                  SameSite=SameSiteMode.Strict

               };
 
            });
             //Alttakileri yorum satırı yapıyoruz onların hepsini tek bir sınıf içinde toplıcaz (unitofwork pattern)
            // services.AddScoped<ICategoryRepository,EfCoreCategoryRepository>(); 
            // services.AddScoped<IProductRepository,EfCoreProductRepository>();
            // services.AddScoped<ICartRepository,EfCoreCartRepository>();
            // services.AddScoped<IOrderRepository,EfCoreOrderRepository>();

            services.AddScoped<IUnitOfWork,UnitOfWork>(); //yukarıdakilerin hepsini bu sınıflar içinde topladık

            services.AddScoped<ICategoryService,CategoryManager>();        
            services.AddScoped<IProductService,ProductManager>();
            services.AddScoped<ICartService,CartManager>();
            services.AddScoped<IOrderService,OrderManager>();



            services.AddScoped<IEmailSender,SmtpEmailSender>();
      
           
            services.AddControllersWithViews();//service lerimizi dahil ediyoruz burda.asp .net core içinde mvc yapısı mevcut.burda viewslerle controllers ları kullanıcagımızı belirtiyoruz
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,IConfiguration configuration,UserManager<User> userManager,RoleManager<IdentityRole> roleManager,ICartService cartService)
        {
            app.UseStaticFiles(); //wwwroot

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider=new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(),"node_modules")),
                    RequestPath="/modules"
            });

            if (env.IsDevelopment())
            {
                
                app.UseDeveloperExceptionPage();
            }
            


            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {




                endpoints.MapControllerRoute(
                    name:"orders",
                    pattern:"orders",
                    defaults: new {controller="Cart",action="GetOrders"}

                 );

                endpoints.MapControllerRoute(
                    name:"checkout",
                    pattern:"checkout",
                    defaults: new {controller="Cart",action="Checkout"}

                 );


                endpoints.MapControllerRoute(
                    name:"cart",
                    pattern:"cart",
                    defaults: new {controller="Cart",action="Index"}

                 );




                 endpoints.MapControllerRoute(
                    name:"adminuseredit",
                    pattern:"admin/user/{id?}",
                    defaults: new {controller="Admin",action="UserEdit"}

                 );
                 
               endpoints.MapControllerRoute(
                    name:"adminusers",
                    pattern:"admin/user/list",
                    defaults: new {controller="Admin",action="UserList"}

                 );


                    endpoints.MapControllerRoute(
                    name:"adminroles",
                    pattern:"admin/role/list",
                    defaults: new {controller="Admin",action="RoleList"}

                 );




                  endpoints.MapControllerRoute(
                    name:"adminrolecreate",
                    pattern:"admin/role/create",
                    defaults: new {controller="Admin",action="RoleCreate"}

                 );

                  endpoints.MapControllerRoute(
                    name:"adminroleedit",
                    pattern:"admin/role/{id?}",
                    defaults: new {controller="Admin",action="RoleEdit"}

                 );



                 endpoints.MapControllerRoute(
                    name:"adminproducts",
                    pattern:"admin/products",
                    defaults: new {controller="Admin",action="ProductList"}

                 );

                    endpoints.MapControllerRoute(
                    name:"adminproductcreate",
                    pattern:"admin/products/create",
                    defaults: new {controller="Admin",action="ProductCreate"}

                 );

                  endpoints.MapControllerRoute(
                    name:"adminproductedit",
                    pattern:"admin/products/{id?}",
                    defaults: new {controller="Admin",action="ProductEdit"}

                 );

                    endpoints.MapControllerRoute(
                    name:"admincategories",
                    pattern:"admin/categories",
                    defaults: new {controller="Admin",action="CategoryList"}

                 );


                    endpoints.MapControllerRoute(
                    name:"admincategorycreate",
                    pattern:"admin/categories/create",
                    defaults: new {controller="Admin",action="CategoryCreate"}

                 );

                    endpoints.MapControllerRoute(
                    name:"admincategoryedit",
                    pattern:"admin/categories/{id?}",
                    defaults: new {controller="Admin",action="CategoryEdit"}

                 );
                 
                


                 endpoints.MapControllerRoute(
                    name:"search",
                    pattern:"search",
                    defaults: new {controller="Shop",action="search"}

                 );

                 endpoints.MapControllerRoute(
                    name:"productdetails",
                    pattern:"{url}",
                    defaults: new {controller="Shop",action="details"}

                 );


                endpoints.MapControllerRoute(
                    name:"products",
                    pattern:"products/{category?}",
                    defaults: new {controller="Shop",action="list"}

                 );


               endpoints.MapControllerRoute(
                   name:"default",
                   pattern:"{controller=Home}/{action=Index}/{id?}" //id de soru işareti olmazsa 500/product/list/2 geçerli olur ama soru işareti koyarsak  500/category/list de geçerli olur ikisi birden geçerli
                 );


            });
        
            SeedIdentity.Seed(userManager,roleManager,cartService,configuration).Wait();
        }
    }
}
