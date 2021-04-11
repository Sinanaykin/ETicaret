namespace shopapp.webui.Models
{
    public class OrderModel
    {
        
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public string Note { get; set; }
        public string Email { get; set; }
        public string CardName { get; set; } //kart bilgileri de gerekiyo checkoutta kullanmak için
        public string CardNumber { get; set; }
        public string ExpirationMonth { get; set; } //son kullanma  ayı               //bu kısımları github da hazır aldığımız cartcontrollerdaki paymentprocess kısmında kullanmak için ekledik.
        public string ExpirationYear { get; set; }//son kullanma  yılı
        public string Cvc { get; set; } //car güvenliği için

        public CartModel CartModel { get; set; } // checkout daki ÖZET BİLGİYİ GÖSTERİRKEN KULLANICAZ BUNU

    }
}