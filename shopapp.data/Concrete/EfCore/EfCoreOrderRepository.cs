using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using shopapp.data.Abstract;
using shopapp.entity;

namespace shopapp.data.Concrete.EfCore
{
    public class EfCoreOrderRepository : EfCoreGenericRepository<Order, ShopContext>, IOrderRepository
    {
        public List<Order> GetOrders(string userId)
        {
            using (var context=new ShopContext())
            {
                var orders=context.Orders     //Orders tablosundan OrderItems tablusuna gidip ordan Product ı aldık
                                  .Include(i=>i.OrderItems)
                                  .ThenInclude(i=>i.Product)
                                  .AsQueryable();
                if (!string.IsNullOrEmpty(userId)) //gönderilen userId boş değilse(eğer boşsa bunu yönetici istiyordur ama boş değilse kullanıcı içindir.)
                {
                    orders=orders.Where(i=>i.UserId==userId);  //eğer userId geliyorsavbu where bloğunu  üstteki sorgu ya ekleriz
                }

                return orders.ToList();
                                  
            }
        }
    }
}