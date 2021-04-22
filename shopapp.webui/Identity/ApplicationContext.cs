
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace shopapp.webui.Identity
{
    //veritabanı ile iletişim sağlıcak aynı ef core deki mantıkla context sınıfı oluşturduk(applicationcontext)
    //applicationcontext dbcontexden türemicek dbcontextin özelleşmiş hali olanIdentityDbContext den türicek
    public class ApplicationContext:IdentityDbContext<User> //shopcontext i dbcontext den türettik applicationcontexti de Identitydbcontext den türetiyoruz  //IdentityUser ve IdentityRole göndermeliyiz ama burda User a ekleme yaptık ondan User ı kullanıcaz 
    {
        //ctor
        public ApplicationContext(DbContextOptions<ApplicationContext> options):base(options) 
        {
            
        }
    }
}