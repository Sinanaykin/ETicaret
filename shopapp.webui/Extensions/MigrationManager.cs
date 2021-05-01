using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using shopapp.data.Concrete.EfCore;
using shopapp.webui.Identity;

namespace shopapp.webui.Extensions
{
    public static class MigrationManager
    {
        public static IHost MigrateDatabase(this IHost host) //Geriye IHost döndürmeli bu metod.ve parantez içindede IHostu genişletmeliyiz.
        {
            using (var scope=host.Services.CreateScope())//scope tanılıyoruz bunu service containeri içinden scope elde ediyoruz
            {
                using (var applicationContext=scope.ServiceProvider.GetRequiredService<ApplicationContext>()) //application contexti alıyoruz burda
                {
                    try
                    {
                        applicationContext.Database.Migrate();//applicationcontext database  üzerinden migration işlemlerini yaparız
                    }
                    catch (System.Exception) //hata varsa bura çalısır
                    {
                        //loglama 
                        throw;
                    }
                }

                using (var shopContext=scope.ServiceProvider.GetRequiredService<ShopContext>())//shopContext contexti alıyoruz burda
                {
                     try
                    {
                        shopContext.Database.Migrate();//shopContext database  üzerinden migration işlemlerini yaparız
                    }
                    catch (System.Exception)
                    {
                        //loglama 
                        throw;
                    }
                }
            }

            return host;
        }
    }
}