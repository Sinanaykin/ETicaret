using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using shopapp.business.Abstract;

namespace shopapp.webui.Identity
{
    public class SeedIdentity
    {
        public static async Task Seed(UserManager<User> userManager,RoleManager<IdentityRole> roleManager,ICartService cartService ,IConfiguration configuration)
        {
            var roles=configuration.GetSection("Data:Roles").GetChildren().Select(x=>x.Value).ToArray(); //Role oluşturmalıyız.GetSection ile Data altındaki Rollere ulaşırız.GetChildren ile alt elemanları alırız ve bunlar üzerinde select ile dolaşırız ve listeleriz
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))//Role bilgisi veritabanıında yoksa 
                {
                    await roleManager.CreateAsync(new IdentityRole(role)); //Rol bilgisini oluşturuyoruz.
                }
                
            }
             
            var users=configuration.GetSection("Data:Users"); //user bilgilerini alıyoruz.Data altındaki users lara ulaşıcaz
            foreach (var section in users.GetChildren()) //users ın altındaki 1. eleman 2. eleman tek tek olusucak
            {
                var username=section.GetValue<string>("username");//username in value bilgisini string olarak geri alıyoruz
                var password=section.GetValue<string>("password");
                var email=section.GetValue<string>("email");
                var role=section.GetValue<string>("role");
                var firstName=section.GetValue<string>("firstName");
                var lastName=section.GetValue<string>("lastName");

                
            if (await userManager.FindByNameAsync(username)==null) //eğer user olustuysa bos değilse user olustur
            {
               
                var user=new User() //user ı olusturuyoruz
                {
                    UserName=username,
                    Email=email,
                    FirstName=firstName,
                    LastName=firstName,
                    EmailConfirmed=true
                };
                var result=await userManager.CreateAsync(user,password);
                if (result.Succeeded) //eğer user ve role olustuysa ,cart bilgisi de olussun diyoruz burda
                {
                    await userManager.AddToRoleAsync(user,role);
                    cartService.InitializeCart(user.Id); 
                }
            }
                
            }


            // var username=configuration["Data:AdminUser:username"];
            // var email=configuration["Data:AdminUser:email"];
            // var password=configuration["Data:AdminUser:password"];
            // var role=configuration["Data:AdminUser:role"];

            // if (await userManager.FindByNameAsync(username)==null)
            // {
            //     await roleManager.CreateAsync(new IdentityRole(role));
            //     var user=new User()
            //     {
            //         UserName=username,
            //         Email=email,
            //         FirstName="sinan",
            //         LastName="sinan",
            //         EmailConfirmed=true
            //     };
            //     var result=await userManager.CreateAsync(user,password);
            //     if (result.Succeeded)
            //     {
            //         await userManager.AddToRoleAsync(user,role);
            //     }
            // }
        }
    }

}