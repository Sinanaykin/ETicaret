using System.Collections.Generic;
using shopapp.business.Abstract;
using shopapp.data.Abstract;
using shopapp.entity;

namespace shopapp.business.Concrete
{
    public class OrderManager : IOrderService
    {
        private IOrderRepository _orderRepository;
        public OrderManager(IOrderRepository orderRepository)
        {
            _orderRepository=orderRepository;
        }
        public void Create(Order entity) //geri dönüş değeri yok return kullanılmaz
        {
           _orderRepository.Create(entity);
        }

        public List<Order> GetOrders(string userId) //geri dönüş değeri var o yüzden return kullanılır
        {
            return _orderRepository.GetOrders(userId);
        }
    }
}