using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using shopapp.business.Abstract;
using System.Threading.Tasks;


namespace shopapp.webui.Components
{
    public class CategoriesViewComponent:ViewComponent
    {
         
       private ICategoryService _categoryService;

       public CategoriesViewComponent(ICategoryService categoryService)
       {
           this._categoryService=categoryService;
       }



        public async Task<IViewComponentResult> InvokeAsync()
        {
            if(RouteData.Values["category"]!=null)
            ViewBag.SelectedCategory=RouteData?.Values["category"];         
               return View(await _categoryService.GetAll());
        }
    }
}