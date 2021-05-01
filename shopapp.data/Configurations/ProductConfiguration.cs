using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using shopapp.entity;

namespace shopapp.data.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(m=>m.ProductId); //ProductId 1 cil anahtar yaptık 
            builder.Property(m=>m.Name).IsRequired().HasMaxLength(100); //Name alanını zorunlu ve max uzunlugu 100 karakter yaptık
            builder.Property(m=>m.DateAdded).HasDefaultValueSql("getdate()");//DateAdded boş geçilirse default değeri getdate fonksiyonu döner.Ama bunu mssql de kullanıyoruz
            //builder.Property(m=>m.DateAdded).HasDefaultValueSql("date('now')");DateAdded boş geçilirse default değeri getdate fonksiyonu döner.Sqlite için bunu kullanıcaz

          
        }
    }
}