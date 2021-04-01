using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using shopapp.business.Abstract;
using shopapp.webui.Identity;
using shopapp.webui.Models;

namespace shopapp.webui.Controllers
{
    [Authorize]
    public class CartController:Controller
    {
       private ICartService _cartService;
       private UserManager<User> _userManager;
       public CartController(ICartService cartService,UserManager<User> userManager)
       {
            _cartService=cartService;
            _userManager=userManager;
       }
        public IActionResult Index() //kullanıcı kartını getirsin
        {
            var cart=_cartService.GetCartByUserId(_userManager.GetUserId(User));//login olan kullanıcının userıd si lazım bize

            return View(new CartModel(){
                CartId=cart.Id,
                CartItems=cart.CartItems.Select(i=>new CartItemModel()
                {
                    CartItemId=i.Id,//CartItems içindeki dolaşıcaz bunlara i dedik
                    ProductId=i.ProductId,
                    Name=i.Product.Name, //efcorecartrepository de cart üzerinden producta geçiş yapmıstık zaten thenınclude ile burdada Name  için product içindeki name diyoruz
                    Price=(double)i.Product.Price,
                    ImageUrl=i.Product.ImageUrl,
                    Quantity=i.Quantity
 
                }).ToList()
            });
        }
        
        [HttpPost]
        public IActionResult AddToCart(int productId,int quantity)//karta bilgi ekler
        {
            var userId=_userManager.GetUserId(User); //userId bilgisini alıyoruz veritabanından

            _cartService.AddToCart(userId,productId,quantity);
            return RedirectToAction("Index");
        }
 
        [HttpPost]
        public IActionResult DeleteFromCart(int productId)
        {
             var userId=_userManager.GetUserId(User); //userId bilgisini alıyoruz veritabanından
            _cartService.DeleteFromCart(userId,productId);
            return RedirectToAction("Index");
        }

    } 
} 