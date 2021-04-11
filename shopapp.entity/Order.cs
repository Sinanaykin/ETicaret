using System;
using System.Collections.Generic;

namespace shopapp.entity
{
    public class Order
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public string UserId { get; set; }
        public string FirstName { get; set; }//userıd ile user tablosundaki isim soyaisimi kullanabiliriz ama siparişi teslim alan kişi farklı olabilir veya sipariş baskasının adına verilmiş olabilir ondan burayıda ekledik
        public string LastName { get; set; }//userıd ile user tablosundaki isim soyaisimi kullanabiliriz ama siparişi teslim alan kişi farklı olabilir veya sipariş baskasının adına verilmiş olabilir ondan burayıda ekledik
        public string Address { get; set; }//userıd ile user tablosundaki isim soyaisimi kullanabiliriz ama siparişi teslim alan kişi farklı olabilir veya sipariş baskasının adına verilmiş olabilir ondan burayıda ekledik
        public string City { get; set; }//userıd ile user tablosundaki isim soyaisimi kullanabiliriz ama siparişi teslim alan kişi farklı olabilir veya sipariş baskasının adına verilmiş olabilir ondan burayıda ekledik
        public string Phone { get; set; }//userıd ile user tablosundaki isim soyaisimi kullanabiliriz ama siparişi teslim alan kişi farklı olabilir veya sipariş baskasının adına verilmiş olabilir ondan burayıda ekledik
        public string Email { get; set; }
        public string Note { get; set; }
        public string PaymentId { get; set; }//payment dan gelen bilgiler için
        public string ConversationId { get; set; } // 
         public EnumPaymentType PaymentType { get; set; } //ödeme secenekleri için enum yaptık bir tane
        public EnumOrderState OrderState { get; set; }//sipariş secenekleri için enum yaptık bir tane
        public List<OrderItem> OrderItems { get; set; } //order ver orderıtem arası 1-cok ilişko var
    }

    public enum EnumPaymentType
    {
        CreditCard=0,
        Eft=1
    }
    public enum EnumOrderState{
       waiting=0,
       unpaid=1,
       completed=2 
    }
}