using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
using shopapp.business.Abstract;
using shopapp.data.Abstract;
using shopapp.webui.Models;
using System.Net.Http;
using System.Threading.Tasks;
using shopapp.entity;
using Newtonsoft.Json;

namespace shopapp.webui.Controllers
{
  
    public class HomeController:Controller 
    {
       private IProductService _productService;

       public HomeController(IProductService productService)
       {
           this._productService=productService;
       }

        public IActionResult Index()
        {
           
 
            var productViewModel=new ProductListViewModel()
            {
               
                Products=_productService.GetHomePageProducts()
            };
            return View(productViewModel);
        }

          public IActionResult About()
        {
            return View();
        }
        
        public async Task<IActionResult> GetProductsFromRestApi()//apiden bütün ürünleri getirmesi için  asekron bir metod olusturduk
        {
            var  products=new List<Product>();//products listesi tanımladık aşağıda doldurucaz bunu ve view de döndürücez
            using(var httpClient=new HttpClient())//Service projesine talebi asp.net core projesinden HttpClient objesi ile gönderebiliriz.HttpClient objesi oluşturuyoruz burdada.
            {
                using (var response=await httpClient.GetAsync("http://localhost:4200/api/products") )//talebe karşılık bir response gelicek.httpclient üzerinden talebi gönderiyoruz.Talebi göndericeğimiz adresin  URL sini giriyoruz. ürünleri alıcağımız için GetAsync kullanıyoruz
                {
                  string apiResponse =await response.Content.ReadAsStringAsync();//oluşan response üzerinden Contenti  string olarak okucaz .String olan bilgi de Json formatında 
                  products=JsonConvert.DeserializeObject<List<Product>>(apiResponse);//sonra okudugumuz yanıtı gelen json formattaki stringi listeye cevirmeliyiz  deserialize(uygulama formatına) ediyoruz.ve apiResponse u gönderip products eşitliyoruz hepsini
                  //serialize:bir listeyi json formata dönüştürür deserialize:json formattan örneğin bir listeye(uygulama formatına yani) ceviriyoruz
                }
            }
 
            return View(products);//products ı geri dönderiyoruz
        }

          public IActionResult Contact()
        {
            return View("MyView");
        }


    }
}  