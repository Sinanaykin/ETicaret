using System;
using System.Collections.Generic;
using System.Linq;
using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using shopapp.business.Abstract;
using shopapp.entity;
using shopapp.webui.Identity;
using shopapp.webui.Models;

namespace shopapp.webui.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private ICartService _cartService;
        private IOrderService _orderService;
        private UserManager<User> _userManager;
        public CartController(ICartService cartService,IOrderService orderService, UserManager<User> userManager)
        {
            _cartService = cartService;
            _orderService=orderService;
            _userManager = userManager;
        }
        public IActionResult Index() //kullanıcı kartını getirsin
        {
            var cart = _cartService.GetCartByUserId(_userManager.GetUserId(User));//login olan kullanıcının userıd si lazım bize

            return View(new CartModel()
            {
                CartId = cart.Id,
                CartItems = cart.CartItems.Select(i => new CartItemModel()
                {
                    CartItemId = i.Id,//CartItems içindeki dolaşıcaz bunlara i dedik
                    ProductId = i.ProductId,
                    Name = i.Product.Name, //efcorecartrepository de cart üzerinden producta geçiş yapmıstık zaten thenınclude ile burdada Name  için product içindeki name diyoruz
                    Price = (double)i.Product.Price,
                    ImageUrl = i.Product.ImageUrl,
                    Quantity = i.Quantity

                }).ToList()
            });
        }

        [HttpPost]
        public IActionResult AddToCart(int productId, int quantity)//karta bilgi ekler
        {
            var userId = _userManager.GetUserId(User); //userId bilgisini alıyoruz veritabanından

            _cartService.AddToCart(userId, productId, quantity);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DeleteFromCart(int productId)
        {
            var userId = _userManager.GetUserId(User); //userId bilgisini alıyoruz veritabanından
            _cartService.DeleteFromCart(userId, productId);
            return RedirectToAction("Index");
        }

        public IActionResult Checkout()
        {

            var cart = _cartService.GetCartByUserId(_userManager.GetUserId(User));//login olan kullanıcının userıd si lazım bize

            var orderModel = new OrderModel();
            orderModel.CartModel = new CartModel()
            {
                CartId = cart.Id,
                CartItems = cart.CartItems.Select(i => new CartItemModel()
                {
                    CartItemId = i.Id,//CartItems içindeki dolaşıcaz bunlara i dedik
                    ProductId = i.ProductId,
                    Name = i.Product.Name, //efcorecartrepository de cart üzerinden producta geçiş yapmıstık zaten thenınclude ile burdada Name  için product içindeki name diyoruz
                    Price = (double)i.Product.Price,
                    ImageUrl = i.Product.ImageUrl,
                    Quantity = i.Quantity

                }).ToList()
            };

            return View(orderModel);


        }

        [HttpPost]
        public IActionResult Checkout(OrderModel model)
        {
            if (ModelState.IsValid)
            {
                var userId=_userManager.GetUserId(User);
               
                var cart=_cartService.GetCartByUserId(userId);
               
               model.CartModel = new CartModel()
                {
                 CartId = cart.Id,
                 CartItems = cart.CartItems.Select(i => new CartItemModel()
                 {
                    CartItemId = i.Id,//CartItems içindeki dolaşıcaz bunlara i dedik
                    ProductId = i.ProductId,
                    Name = i.Product.Name, //efcorecartrepository de cart üzerinden producta geçiş yapmıstık zaten thenınclude ile burdada Name  için product içindeki name diyoruz
                    Price = (double)i.Product.Price,
                    ImageUrl = i.Product.ImageUrl,
                    Quantity = i.Quantity

                 }).ToList()
                };
                 
                 var payment=PaymentProcess(model);

                if (payment.Status=="success")
                {
                  SaveOrder(model,payment,userId);//eğer ödeme başarılı ise cehckout dan girilen bilgileri siparişe çevirmeliyiz.
                  ClearCart(model.CartModel.CartId);
                 return View("Success");
                }else
                {
                     var msg=new AlertMessage() //hata mesajını gönderelim bu yapıyı önceden admincontroller da kullanmıstık zaten
                     {
       
                       Message=  $"{payment.ErrorMessage} ",
                       AlertType="danger"
  
                     };
                       TempData["message"]= JsonConvert.SerializeObject(msg);
                     }
                }
         
               return View(model);
            }

           public IActionResult GetOrders()
            {
                var userId=_userManager.GetUserId(User);//userId alıyoruz burda 
                var orders=  _orderService.GetOrders(userId); //veritabanından ilgili userId ye ait sipariş bilgileri gelir


                var orderListModel= new List<OrderListModel>();
                OrderListModel orderModel;
                foreach (var order in orders) //veritabanından gelen orders bilgilerini model aktarıcaaz ordanda view gidicek.
                {
                    orderModel=new OrderListModel();

                    orderModel.OrderId=order.Id;
                    orderModel.OrderNumber=order.OrderNumber;
                    orderModel.OrderDate=order.OrderDate;
                    orderModel.Phone=order.Phone;
                    orderModel.FirstName=order.FirstName;
                    orderModel.LastName=order.LastName;
                    orderModel.Email=order.Email;
                    orderModel.Address=order.Address;
                    orderModel.City=order.City;
                    orderModel.OrderState=order.OrderState;
                    orderModel.PaymentType=order.PaymentType;
                    

                    orderModel.OrderItems=order.OrderItems.Select(i=>new OrderItemModel(){
                        OrderItemId=i.Id,
                        Name=i.Product.Name,
                        Price=(double)i.Price,
                        Quantity=i.Quantity,
                        ImageUrl=i.Product.ImageUrl
                    

                    }).ToList();

                    orderListModel.Add(orderModel);
                    
                }


                return View("Orders",orderListModel);//bunları model içinde paketleyip view üzerinden kolay ulasabilmeliyiz
            }

        private void ClearCart(int cartId)
        {
            _cartService.ClearCart(cartId);
        }

        private void SaveOrder(OrderModel model, Payment payment, string userId)
        {
            var order =new Order();
            order.OrderNumber=new Random().Next(111111,999999).ToString();//sipariş no için 6 haneli ve bu sayılar arasında bir random sayı üretir.
            order.OrderState=EnumOrderState.completed;
            order.PaymentType=EnumPaymentType.CreditCard;
            order.PaymentId=payment.PaymentId;
            order.ConversationId=payment.ConversationId;
            order.OrderDate=new DateTime();
            order.FirstName=model.FirstName;
            order.LastName=model.LastName;
            order.UserId=userId;
            order.Address=model.Address;
            order.Phone=model.Phone;
            order.Email=model.Email;
            order.City=model.City;
            order.Note=model.Note;
            
            order.OrderItems=new List<entity.OrderItem>();//OrderItems ı altta alıyoruz.CartItems ı tek tek dolaşarak OrderItems ı oluşturucaz.
           foreach (var item in model.CartModel.CartItems)
            {
                var orderItem=new shopapp.entity.OrderItem()
                {
                    Price=item.Price,
                    Quantity=item.Quantity,
                    ProductId=item.ProductId

                };
               
                order.OrderItems.Add(orderItem);//order içindeki OrderItems a orderItem ı ekle
            }
            _orderService.Create(order);//order ı oluştur database de
        }

        private Payment PaymentProcess(OrderModel model)
        {
            Options options = new Options();
            options.ApiKey = "sandbox-viR9GE8ZCgrDhwtaAPl8UWaEp8yaFfAb";
            options.SecretKey = "sandbox-xipOMbw1MoUiw37ZjYVR6uMiWw5aTFTi";
            options.BaseUrl = "https://sandbox-api.iyzipay.com";

            CreatePaymentRequest request = new CreatePaymentRequest();
            request.Locale = Locale.TR.ToString();
            request.ConversationId = new Random().Next(111111111,999999999).ToString(); //random bir sayı üretsin
            request.Price =model.CartModel.TotalPrice().ToString();
            request.PaidPrice = model.CartModel.TotalPrice().ToString();
            request.Currency = Currency.TRY.ToString();
            request.Installment = 1;
            request.BasketId = "B67832";
            request.PaymentChannel = PaymentChannel.WEB.ToString();
            request.PaymentGroup = PaymentGroup.PRODUCT.ToString();

            PaymentCard paymentCard = new PaymentCard(); //kart bilgilerini alıyoruz burda
            paymentCard.CardHolderName = model.CardName;
            paymentCard.CardNumber = model.CardNumber;
            paymentCard.ExpireMonth = model.ExpirationMonth;
            paymentCard.ExpireYear = model.ExpirationYear;
            paymentCard.Cvc = model.Cvc;
            paymentCard.RegisterCard = 0;
            request.PaymentCard = paymentCard;

            // paymentCard.CardNumber = "5528790000000008";
            // paymentCard.ExpireMonth = "12";
            // paymentCard.ExpireYear = "2030";
            // paymentCard.Cvc = "123";

            Buyer buyer = new Buyer(); //ürünü alan kişi bilgileri için
            buyer.Id = "BY789";
            buyer.Name = model.FirstName;
            buyer.Surname = model.LastName;
            buyer.GsmNumber = "+905350000000";
            buyer.Email = "email@email.com";
            buyer.IdentityNumber = "74300864791";
            buyer.LastLoginDate = "2015-10-05 12:43:35";
            buyer.RegistrationDate = "2013-04-21 15:12:09";
            buyer.RegistrationAddress = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            buyer.Ip = "85.34.78.112";
            buyer.City = "Istanbul";
            buyer.Country = "Turkey";
            buyer.ZipCode = "34732";
            request.Buyer = buyer;

            Address shippingAddress = new Address();
            shippingAddress.ContactName = "Jane Doe";
            shippingAddress.City = "Istanbul";
            shippingAddress.Country = "Turkey";
            shippingAddress.Description = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            shippingAddress.ZipCode = "34742";
            request.ShippingAddress = shippingAddress;

            Address billingAddress = new Address();
            billingAddress.ContactName = "Jane Doe";
            billingAddress.City = "Istanbul";
            billingAddress.Country = "Turkey";
            billingAddress.Description = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            billingAddress.ZipCode = "34742";
            request.BillingAddress = billingAddress;

            List<BasketItem> basketItems = new List<BasketItem>();
            BasketItem basketItem;

            foreach (var item in model.CartModel.CartItems)
            {
                basketItem=new BasketItem();
                basketItem.Id=item.ProductId.ToString();
                basketItem.Name=item.Name;
                basketItem.Category1="Technologic Device";
                basketItem.Price=(item.Price*item.Quantity).ToString();
                basketItem.ItemType=BasketItemType.PHYSICAL.ToString();
                basketItems.Add(basketItem);
            }  
            request.BasketItems = basketItems;

            return Payment.Create(request, options);

        }
    }
}