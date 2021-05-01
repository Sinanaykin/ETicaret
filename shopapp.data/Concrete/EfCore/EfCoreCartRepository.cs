using Microsoft.EntityFrameworkCore;
using shopapp.data.Abstract;
using shopapp.entity;
using System.Linq;

namespace shopapp.data.Concrete.EfCore
{
    public class EfCoreCartRepository : EfCoreGenericRepository<Cart>, ICartRepository
    {
         public EfCoreCartRepository(ShopContext context): base(context)
       {
           
       }
       private ShopContext ShopContext
       {
          get {return context as ShopContext; }
       }


        public void ClearCart(int cartId)
        {
            
              var cmd = @"delete from CartItems where CartId=@p0";
              ShopContext.Database.ExecuteSqlRaw(cmd,cartId);
            
        }

        public void DeleteFromCart(int cartId, int productId)
        {
          
              var cmd = @"delete from CartItems where CartId=@p0 and ProductId=@p1";
              ShopContext.Database.ExecuteSqlRaw(cmd,cartId,productId);
          
        }

        public Cart GetByUserId(string userId)
        {
           
               return ShopContext.Carts  //context üzerinden cart bbilgilerine geciş yapıyoruz
                       .Include(i=>i.CartItems) //cart içindeki cart ıtems lar gelicek
                       .ThenInclude(i=>i.Product)//cartıtems danda product a ulasabiliyoruz
                       .FirstOrDefault(i=>i.UserId==userId);//bunu where içinde yapıp sonra firstordefault diyebilirim
           
        }

        public override void Update(Cart entity)//Carta ait bilgiler güncellenicek
        {
           
                ShopContext.Carts.Update(entity);
             
            
            
        }
    }
}