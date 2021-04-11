using shopapp.business.Abstract;
using shopapp.data.Abstract;
using shopapp.entity;

namespace shopapp.business.Concrete
{
    public class CartManager : ICartService
    {
        private ICartRepository _cartRepository;
        public CartManager(ICartRepository cartRepository)
        {
            _cartRepository=cartRepository;
        }

        public void AddToCart(string userId, int productId, int quantity)
        {
           var cart=GetCartByUserId(userId);//kullanıcının kartını alıyoruz 
           if (cart!=null)
           {
               //eklenmek isteyen ürün sepette varmı(güncelleme)
                //sepette var ve yeni kayıt olustur(ekle)
               var index=cart.CartItems.FindIndex(i=>i.ProductId==productId);//sepete eklenmek istenen ürün varsa sepette index değeri gelir int olarak
               if (index<0)//eğer sepete eklenen ürün yoksa ürün ekle
               {
                   cart.CartItems.Add(new CartItem(){
                       ProductId=productId,
                       Quantity=quantity,
                       CartId=cart.Id

                   });
               }
               else //eğer sepete eklenen ürün sepette varsa quantity arttır
               {
                   cart.CartItems[index].Quantity += quantity;
               }
               _cartRepository.Update(cart);
           
           }
        }

        public void ClearCart(int cartId)
        {
            _cartRepository.ClearCart(cartId);
        }

        public void DeleteFromCart(string userId, int productId)
        {
            var cart=GetCartByUserId(userId);
            if (cart!=null)
            {
                _cartRepository.DeleteFromCart(cart.Id,productId);
            }
        }

        public Cart GetCartByUserId(string userId)
        {
            return _cartRepository.GetByUserId(userId); //data katmanına gidip bu bilgiyi alıcaz.
        }

        public void InitializeCart(string userId)
        {
            _cartRepository.Create(new Cart(){UserId=userId});
        }
    }
}