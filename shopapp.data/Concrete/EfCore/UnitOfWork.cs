using shopapp.data.Abstract;

namespace shopapp.data.Concrete.EfCore
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ShopContext _context; //contextimizi belirttik 
        public UnitOfWork(ShopContext context) //ctor yaptık
        {
            _context=context;
        }
        //her public üyeni private versinunu oluşturucaz 
        private EfCoreCartRepository _cartRepository; 
        private EfCoreCategoryRepository _categoryRepository;
        private EfCoreOrderRepository _orderRepository;
        private EfCoreProductRepository _productRepository;


        public ICartRepository Carts =>
         _cartRepository=_cartRepository ?? new EfCoreCartRepository(_context); //_cartrepository varsa geriye _cartrepository gönderilicek eğer yoksa null ise EfcoreCartRepository geri alıcaz içine de contexi veri

        public ICategoryRepository Categories => 
        _categoryRepository=_categoryRepository ?? new EfCoreCategoryRepository(_context);

        public IOrderRepository Orders => _orderRepository=_orderRepository ?? new EfCoreOrderRepository(_context);

        public IProductRepository Products =>_productRepository=_productRepository ?? new EfCoreProductRepository(_context);

        public void Dispose()
        {
            _context.Dispose();
        }

        public void Save() //save içine ;context Yani ShopContext i  savechanges yap (kaydet) dedik
        {
            //yani save i çağırınca _context.savechanges çalışır.
            _context.SaveChanges();
        }
    }
}