using Microsoft.AspNetCore.Mvc;
using shopapp.webui.Models;
using shopapp.business.Abstract;
using System.Linq;
using shopapp.entity;
using Newtonsoft.Json;
using System.IO;
using Microsoft.AspNetCore.Http;
using System;
using shopapp.webui.Extensions;
using Microsoft.AspNetCore.Identity;
using shopapp.webui.Identity;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

namespace shopapp.webui.Controllers
{
    //sinanaykin=>admin
    //sliveli,ayseözgü=>customer
    [Authorize(Roles="admin")]

    public class AdminController:Controller
    {
     private IProductService _productService;
      private ICategoryService _categoryService;
      private RoleManager<IdentityRole> _roleManager;
      private UserManager<User> _userManager;

       public AdminController(IProductService productService,ICategoryService categoryService,RoleManager<IdentityRole> roleManager,UserManager<User> userManager)
       {
           this._productService=productService;
           this._categoryService=categoryService;
           this._roleManager=roleManager;
           this._userManager=userManager;
       }

       [HttpGet]
       public async Task<IActionResult> UserEdit(string id)
       {      
           var user =await _userManager.FindByIdAsync(id);
           if (user!=null)
           {
               var selectedRoles=await _userManager.GetRolesAsync(user);
               var roles=_roleManager.Roles.Select(i=>i.Name);
               
               ViewBag.Roles=roles;
               return View(new  UserDetailsModel(){
                   UserId=user.Id,
                   UserName=user.UserName,
                   FirstName=user.FirstName,
                   LastName=user.LastName,
                   Email=user.Email,
                   EmailConfirmed=user.EmailConfirmed,
                   SelectedRoles=selectedRoles
               });

           }
           return Redirect("~/admin/user/list");
       }
       
       [HttpPost]
       public async Task<IActionResult> UserEdit(UserDetailsModel model,string[] selectedRoles)
       {   
           if (ModelState.IsValid)
           {
               var user=await _userManager.FindByIdAsync(model.UserId);
               if (user!=null)
               {
                   user.FirstName=model.FirstName;
                   user.LastName=model.LastName;
                   user.UserName=model.UserName;
                   user.Email=model.Email;
                   user.EmailConfirmed=model.EmailConfirmed;

                   var result=await _userManager.UpdateAsync(user);
                   if (result.Succeeded)
                   {          
                      var userRoles=await _userManager.GetRolesAsync(user);  
                      selectedRoles=selectedRoles?? new string[]{};
                      await _userManager.AddToRolesAsync(user,selectedRoles.Except(userRoles).ToArray<string>());
                      await _userManager.RemoveFromRolesAsync(user,userRoles.Except(selectedRoles).ToArray<string>());
                      
                      return Redirect("/admin/user/list");
                   }
               }
                return Redirect("/admin/user/list");
           }
           return View(model);

       }
       

       public IActionResult UserList()
       {
          
           return View(_userManager.Users);
       }
       
       [HttpGet]
       public async Task<IActionResult> RoleEdit(string id)
       {
           var role=await _roleManager.FindByIdAsync(id);
           var members=new List<User>();
           var nonmembers=new List<User>();
           foreach (var user in _userManager.Users)
           {
             var list= await _userManager.IsInRoleAsync(user,role.Name)?members:nonmembers;
             list.Add(user);
           }
           var model=new RoleDetailsModel()
           {
               Role=role,
               Members=members,
               NonMembers=nonmembers
           };
           return View(model);
       }

        [HttpPost]
        public async Task<IActionResult> RoleEdit(RoleEditModel model)
       {
           if (ModelState.IsValid)
           {
               foreach (var userId in model.IdsToAdd ??new string[]{})
               {
                   var user=await _userManager.FindByIdAsync(userId);
                   if (user!=null)
                   {
                       var result= await _userManager.AddToRoleAsync(user,model.RoleName);
                       if (!result.Succeeded)
                       {
                          foreach (var error in result.Errors)
                          {
                              ModelState.AddModelError("",error.Description);
                          } 
                       }
                   }
               }
           
                foreach (var userId in model.IdsToDelete ??new string[]{})
               {
                   var user=await _userManager.FindByIdAsync(userId);
                   if (user!=null)
                   {
                       var result=await _userManager.RemoveFromRoleAsync(user,model.RoleName);
                       if (!result.Succeeded)
                       {
                          foreach (var error in result.Errors)
                          {
                              ModelState.AddModelError("",error.Description);
                          } 
                       }
                   }
               }
           }
           return Redirect("/admin/role/" +model.RoleId);

       }

       

       public IActionResult RoleList()
       {
           return View(_roleManager.Roles);
       }

       [HttpGet]
       public IActionResult RoleCreate()
       {
           return View();
       }
       [HttpPost]
        public async Task<IActionResult> RoleCreate(RoleModel model)
       {
           if (ModelState.IsValid)
           {
               var result=await _roleManager.CreateAsync(new IdentityRole(model.Name));
               if (result.Succeeded)
               {
                   return RedirectToAction("RoleList");
               }else
               {
                   foreach (var error in result.Errors)
                   {
                      ModelState.AddModelError("",error.Description);   
                   }
               } 
           }
           return View(model);
       }
        public async Task<IActionResult> ProductList()
        {
            var products=await _productService.GetAll();
            return View(new ProductListViewModel()
            {
                Products=products

            });
        } 


        public async Task<IActionResult> CategoryList()
        {
            var categories=await _categoryService.GetAll();
            return View(new CategoryListViewModel()
            {
                Categories=categories
            });
        }
        
        [HttpGet]
        public IActionResult ProductCreate() //productcreate sayfası döner viewdeki boş şekilde gelir çünkü içini çekmemişiz veritabanından update deki gibi
        {
            return View();

        }

          [HttpPost]
        public async Task<IActionResult> ProductCreate(ProductModel model,IFormFile file) //FORMDAKİ İNPUTTAN GÖNDERİLEN VERİLER VERİTABANINDAN GELEN VERİLERE EKLENİR
        {
            if (ModelState.IsValid)
            {
                var entity=new Product();
                
                    entity.Name=model.Name;
                    entity.Url=model.Url;
                    entity.Price=model.Price;
                    entity.Description=model.Description;
                    if (file!=null)
                     {
                
                       var extention=Path.GetExtension(file.FileName);
                       var randomName=string.Format($"{Guid.NewGuid()}{extention}");
                       entity.ImageUrl=randomName;
                       var path = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot\\img",randomName);
                       using (var stream = new FileStream(path,FileMode.Create))
                        {   
                          await file.CopyToAsync(stream);
                        }
                     }
                    
                
            
           if( _productService.Create(entity))
           {
                TempData.Put("message",new AlertMessage()
                {
                    Title="Kayıt Eklendi.",
                    Message="Kayıt Eklendi.",
                    AlertType="success"

                });
            
            return RedirectToAction("ProductList");

            }
            TempData.Put("message",new AlertMessage()
                {
                    Title="Hata.",
                    Message="_productService.ErrorMessage",
                    AlertType="danger"

                });
            
            
            return View(model);

           }
   
            return View(model);
            
        }

        
        [HttpGet]
        public IActionResult CategoryCreate()
        {
            return View();

        }

          [HttpPost]
        public IActionResult CategoryCreate(CategoryModel model)
        {
            if (ModelState.IsValid)
            {
                var entity=new Category()
                {
                    Name=model.Name,
                    Url=model.Url
                    
                };
            
            _categoryService.Create(entity);
            TempData.Put("message",new AlertMessage()
                {
                    Title="Kayıt Eklendi.",
                    Message=$"{entity.Name} isimli category eklendi.",
                    AlertType="success"

                });
            
            return RedirectToAction("CategoryList");

            }
            return View(model);
            
        }

         [HttpGet]
        public async Task<IActionResult> ProductEdit(int? id)
        {
            if (id==null)
            {
                return NotFound();
            }
            var entity=_productService.GetByIdWithCategories((int)id);
            if (entity==null)
            {
                 return NotFound();
            }
              var model=new ProductModel()
              {
                  ProductId=entity.ProductId,
                  Name=entity.Name,
                  Url=entity.Url,
                  Price=entity.Price,
                  ImageUrl=entity.ImageUrl,
                  Description=entity.Description,
                  IsApproved=entity.IsApproved,
                  IsHome=entity.IsHome,
                  SelectedCategories=entity.ProductCategories.Select(i=>i.Category).ToList()

              };
              ViewBag.Categories=await _categoryService.GetAll();


            return View(model);

        }

         [HttpPost]
        public async Task<IActionResult> ProductEdit(ProductModel model,int[] categoryIds,IFormFile file)
        {
            if (ModelState.IsValid)
            {
                 var entity= await _productService.GetById(model.ProductId);
            if (entity==null)
            {
                return NotFound();
            }
            entity.Name=model.Name;
            entity.Price=model.Price;
            entity.Url=model.Url;
                    
            entity.Description=model.Description;
            entity.IsHome=model.IsHome;
            entity.IsApproved=model.IsApproved;
            if (file!=null)
            {
                
                var extention=Path.GetExtension(file.FileName);
                var randomName=string.Format($"{Guid.NewGuid()}{extention}");
                entity.ImageUrl=randomName;
                var path = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot\\img",randomName);
                using (var stream = new FileStream(path,FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }

             if( _productService.Update(entity,categoryIds))
           {
                TempData.Put("message",new AlertMessage()
                {
                    Title="Kayıt Güncellendi.",
                    Message="Kayıt Güncellendi.",
                    AlertType="success"

                });
          
            return RedirectToAction("ProductList");

            }
              TempData.Put("message",new AlertMessage()
                {
                    Title="Hata.",
                    Message="_productService.ErrorMessage",
                    AlertType="danger"

                });
                       
            }
            ViewBag.Categories= await _categoryService.GetAll();

            return View(model);
           

        }

         [HttpGet]
        public IActionResult CategoryEdit(int? id)
        {
            if (id==null)
            {
                return NotFound();
            }
            var entity=_categoryService.GetByIdWithProducts((int)id);
            if (entity==null)
            {
                 return NotFound();
            }
              var model=new CategoryModel()
              {
                  CategoryId=entity.CategoryId,
                  Name=entity.Name,
                  Url=entity.Url,
                  Products=entity.ProductCategories.Select(p=>p.Product).ToList()
                 

              };
            return View(model);

        }

         [HttpPost]
        public async Task<IActionResult> CategoryEdit(CategoryModel model)
        {
            if (ModelState.IsValid)
            {
                 var entity= await _categoryService.GetById(model.CategoryId);
                if (entity==null)
                {
                 return NotFound();
                 }
                  entity.Name=model.Name;           
                  entity.Url=model.Url;
           
                _categoryService.Update(entity);
                var msg=new AlertMessage()
                {
       
                 Message=  $"{entity.Name} isimli category güncellendi.",
                 AlertType="success"

                 };
                 TempData["message"]= JsonConvert.SerializeObject(msg);
                 return RedirectToAction("CategoryList");
                
            }
              
              return View(model);
           

        }


        public async Task<IActionResult> DeleteProduct(int productId)
        {
        var entity= await _productService.GetById(productId);
        
        if (entity!=null)
        {
            _productService.Delete(entity);
        }
         var msg=new AlertMessage()
            {
       
            Message=  $"{entity.Name} isimli ürün silindi.",
            AlertType="danger"

           };
            
             TempData["message"]= JsonConvert.SerializeObject(msg);
            return RedirectToAction("ProductList");

        }


        
        public async Task<IActionResult> DeleteCategory(int categoryId)
        {
        var entity=await _categoryService.GetById(categoryId);
        
        if (entity!=null)
        {
            _categoryService.Delete(entity);
        }
         var msg=new AlertMessage()
            {
       
            Message=  $"{entity.Name} isimli category silindi.",
            AlertType="danger"

           };
            
             TempData["message"]= JsonConvert.SerializeObject(msg);
            return RedirectToAction("CategoryList");

        }

        [HttpPost]
        public IActionResult DeleteFromCategory(int productId,int categoryId)
        {
            _categoryService.DeleteFromCategory(productId,categoryId);
            return Redirect("/admin/categories/"+categoryId);

        }

        
    }
}