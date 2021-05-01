using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using shopapp.entity;

namespace shopapp.data.Configurations
{
    public class ProductCategoryConfiguration : IEntityTypeConfiguration<ProductCategory>
    {
        public void Configure(EntityTypeBuilder<ProductCategory> builder)
        {
             builder.HasKey(c=>new {c.CategoryId,c.ProductId});//CategoryId ve ProductId yi 1 cil anahtar yaptık .Shopcontext içindeki OnModelCreating fonksiyonu içinden çağırdık bu classı 
             
           
        }

    
    }
}