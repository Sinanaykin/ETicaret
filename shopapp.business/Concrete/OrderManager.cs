using System.Collections.Generic;
using shopapp.business.Abstract;
using shopapp.data.Abstract;
using shopapp.entity;

namespace shopapp.business.Concrete
{
    public class OrderManager : IOrderService
    {
       
       private readonly IUnitOfWork _unitofwork;
        public OrderManager(IUnitOfWork unitofwork)
        {
            _unitofwork=unitofwork;
        }
        public void Create(Order entity) //geri dönüş değeri yok return kullanılmaz
        {
           _unitofwork.Orders.Create(entity);
           _unitofwork.Save();
        }

        public List<Order> GetOrders(string userId) //geri dönüş değeri var o yüzden return kullanılır
        {
            return _unitofwork.Orders.GetOrders(userId);
        }
    }
}