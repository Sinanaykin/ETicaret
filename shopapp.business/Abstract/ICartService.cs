using shopapp.entity;

namespace shopapp.business.Abstract
{
    public interface ICartService
    {
         void InitializeCart(string userId);//kullanıcı kartı olusturmak için giriş yapınca
         Cart GetCartByUserId(string userId);//UserId ye göre  Cart alıcaz
       
         void AddToCart(string userId,int productId,int quantity);//dısarıdan alıcağı değerler
        void DeleteFromCart(string userId, int productId);
        void ClearCart(int cartId);//cartId bilgisi ile silme işlemi yapalım
    }
}