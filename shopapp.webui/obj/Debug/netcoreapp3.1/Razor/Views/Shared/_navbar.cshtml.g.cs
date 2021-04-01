#pragma checksum "C:\Users\dbhsoft\Desktop\Eticaret-master\shopapp.webui\Views\Shared\_navbar.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "fdf208e014a1fe94795d184f979f085b8b8fd159"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared__navbar), @"mvc.1.0.view", @"/Views/Shared/_navbar.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 3 "C:\Users\dbhsoft\Desktop\Eticaret-master\shopapp.webui\Views\_ViewImports.cshtml"
using shopapp.entity;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "C:\Users\dbhsoft\Desktop\Eticaret-master\shopapp.webui\Views\_ViewImports.cshtml"
using shopapp.webui.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "C:\Users\dbhsoft\Desktop\Eticaret-master\shopapp.webui\Views\_ViewImports.cshtml"
using Newtonsoft.Json;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "C:\Users\dbhsoft\Desktop\Eticaret-master\shopapp.webui\Views\_ViewImports.cshtml"
using shopapp.webui.Extensions;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "C:\Users\dbhsoft\Desktop\Eticaret-master\shopapp.webui\Views\_ViewImports.cshtml"
using Microsoft.AspNetCore.Identity;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "C:\Users\dbhsoft\Desktop\Eticaret-master\shopapp.webui\Views\_ViewImports.cshtml"
using shopapp.webui.Identity;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"fdf208e014a1fe94795d184f979f085b8b8fd159", @"/Views/Shared/_navbar.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"1ba1f3f088b3562fa6c426d16e510b9f1e97eec6", @"/Views/_ViewImports.cshtml")]
    public class Views_Shared__navbar : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral(@"
 <div class=""navbar bg-danger navbar-dark navbar-expand-sm""> 
         
         
            <a href=""/"" class=""navbar-brand"">Shopapp</a> 
            <ul class=""navbar-nav mr-auto"">
                <li class=""nav-item""> 
                    <a href=""/products"" class=""nav-link ml-2"">Products</a>
                </li>

");
#nullable restore
#line 11 "C:\Users\dbhsoft\Desktop\Eticaret-master\shopapp.webui\Views\Shared\_navbar.cshtml"
                 if (User.Identity.IsAuthenticated) //giriş yapan görebilir(customer) ve giriş yapmış olup  admin rolünde olanlar  da görebilir
                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                <li class=\"nav-item\">\n                    <a href=\"/cart\" class=\"nav-link\">Cart</a>\n                </li>\n                 <li class=\"nav-item\">\n                    <a");
            BeginWriteAttribute("href", " href=\"", 667, "\"", 674, 0);
            EndWriteAttribute();
            WriteLiteral(" class=\"nav-link\">Orders</a>\n                </li>\n");
#nullable restore
#line 19 "C:\Users\dbhsoft\Desktop\Eticaret-master\shopapp.webui\Views\Shared\_navbar.cshtml"
                if (User.IsInRole("admin"))//giriş  yapmış olup sadece admin rolünde olanlar görebilir
                {

#line default
#line hidden
#nullable disable
            WriteLiteral(@"                <li class=""nav-item"">
                   <a href=""/admin/products"" class=""nav-link"">Admin Products</a>
                </li>
                 <li class=""nav-item"">
                   <a href=""/admin/categories"" class=""nav-link"">Admin Categories</a>
                </li>
                 <li class=""nav-item"">
                   <a href=""/admin/role/list"" class=""nav-link"">Roles</a>
                </li>
                 <li class=""nav-item"">
                   <a href=""/admin/user/list"" class=""nav-link"">Users</a>
                </li>
");
#nullable restore
#line 33 "C:\Users\dbhsoft\Desktop\Eticaret-master\shopapp.webui\Views\Shared\_navbar.cshtml"

                }
                
                }

#line default
#line hidden
#nullable disable
            WriteLiteral("            </ul>           \n\n\n            <ul class=\"navbar-nav ml-auto\">\n");
#nullable restore
#line 41 "C:\Users\dbhsoft\Desktop\Eticaret-master\shopapp.webui\Views\Shared\_navbar.cshtml"
                 if (User.Identity.IsAuthenticated) //sadece giriş yapan görür
                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                    <li class=\"nav-item\">\n                    <a href=\"/account/manage\" class=\"nav-link\">");
#nullable restore
#line 44 "C:\Users\dbhsoft\Desktop\Eticaret-master\shopapp.webui\Views\Shared\_navbar.cshtml"
                                                          Write(User.Identity.Name);

#line default
#line hidden
#nullable disable
            WriteLiteral("</a>\n                    </li> \n");
            WriteLiteral("                    <li class=\"nav-item\">\n                    <a href=\"/account/logout\" class=\"nav-link\">Logout</a>\n                    </li>  \n");
#nullable restore
#line 50 "C:\Users\dbhsoft\Desktop\Eticaret-master\shopapp.webui\Views\Shared\_navbar.cshtml"
                }
                else 
                { //sadece giriş yapmayan görür

#line default
#line hidden
#nullable disable
            WriteLiteral(@"                   <li class=""nav-item"">
                    <a href=""/account/login"" class=""nav-link"">Login</a> 
                   </li>
                   <li class=""nav-item"">
                    <a href=""/account/register"" class=""nav-link"">Register</a>
                   </li>
");
#nullable restore
#line 59 "C:\Users\dbhsoft\Desktop\Eticaret-master\shopapp.webui\Views\Shared\_navbar.cshtml"
                }

#line default
#line hidden
#nullable disable
            WriteLiteral("               \n                \n            \n            </ul>           \n        \n    </div>\n");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
