using Microsoft.EntityFrameworkCore;
using shopapp.data.Configurations;
using shopapp.entity;

namespace shopapp.data.Concrete.EfCore
{
      public class ShopContext:DbContext
    {
        //Application contextde oldugu gibi yaptık
        //Herhangi bir arayüz uygulamasından ilgili connection stringi uygulamaya tanıtarak dısarıdan coontexte bir connection string göndererek uygullamadan veritabanına bağlantı sağlarız
        public ShopContext(DbContextOptions<ShopContext> options): base(options) 
        {
            
        }
        public DbSet<Product> Products {get;set;}
        public DbSet<Category> Categories {get;set;}
        public DbSet<Cart> Carts {get;set;}
        public DbSet<CartItem> CartItems {get;set;}
        public DbSet<Order> Orders {get;set;}
        public DbSet<OrderItem> OrderItems {get;set;}

        // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        // {
        //     optionsBuilder.UseSqlite("Data Source=shopDb");
        // }
        protected override void OnModelCreating(ModelBuilder modelBuilder)//normalde buraya yazıyoduk fluent api leri ama cok fazla entity var diye Configurations diye klasör oluşturup onun içinden aldık fluent apileri
        {
          modelBuilder.ApplyConfiguration(new ProductConfiguration()); //Configurations altındaki ProductConfiguration nu aldık .Yani Product ın  fluent api özelliklerini
          modelBuilder.ApplyConfiguration(new CategoryConfiguration());
          modelBuilder.ApplyConfiguration(new ProductCategoryConfiguration());//Configurations altındaki ProductCategoryConfiguration nu aldık .Yani ProductCategory nın  fluent api özelliklerini
          
          modelBuilder.Seed();//tEST VERİLERİNİ EKLEMEK İÇİN .Uygulamayı yayınladığımızda burayı yorum satırı yapmalıyız
          
        }
    }
}