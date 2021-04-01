using shopapp.entity;
using System.Collections.Generic;

namespace shopapp.data.Abstract
{
    public interface ICategoryRepository:IRepository<Category>
    {
      Category GetByIdWithProducts(int categoryId);
      void DeleteFromCategory(int productId,int categoryId);
    }
}