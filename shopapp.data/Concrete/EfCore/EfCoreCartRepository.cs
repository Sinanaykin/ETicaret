using Microsoft.EntityFrameworkCore;
using shopapp.data.Abstract;
using shopapp.entity;
using System.Linq;

namespace shopapp.data.Concrete.EfCore
{
    public class EfCoreCartRepository : EfCoreGenericRepository<Cart, ShopContext>, ICartRepository
    {
        public void ClearCart(int cartId)
        {
            using (var context=new ShopContext())
            {
              var cmd = @"delete from CartItems where CartId=@p0";
              context.Database.ExecuteSqlRaw(cmd,cartId);
            }
        }

        public void DeleteFromCart(int cartId, int productId)
        {
          using(var context=new ShopContext())
          {
              var cmd = @"delete from CartItems where CartId=@p0 and ProductId=@p1";
              context.Database.ExecuteSqlRaw(cmd,cartId,productId);
          }
        }

        public Cart GetByUserId(string userId)
        {
           using(var context=new ShopContext())
           {
               return context.Carts  //context üzerinden cart bbilgilerine geciş yapıyoruz
                       .Include(i=>i.CartItems) //cart içindeki cart ıtems lar gelicek
                       .ThenInclude(i=>i.Product)//cartıtems danda product a ulasabiliyoruz
                       .FirstOrDefault(i=>i.UserId==userId);//bunu where içinde yapıp sonra firstordefault diyebilirim
           }
        }

        public override void Update(Cart entity)//Carta ait bilgiler güncellenicek
        {
            using (var context=new ShopContext())
            {
                context.Carts.Update(entity);
                context.SaveChanges();
            }
            
        }
    }
}