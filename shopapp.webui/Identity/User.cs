using Microsoft.AspNetCore.Identity;

namespace shopapp.webui.Identity
{
    public class User:IdentityUser            //istersek identity altına Role clası acıp onuda özelleştirip bu şekilde IdentityRole den türetebiliriz
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        
        
    }
}