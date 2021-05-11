using System.Collections.Generic;
using System.Threading.Tasks;
using shopapp.entity;

namespace shopapp.business.Abstract
{
    public interface ICategoryService:IValidator<Category>
    {
         
        Task<Category> GetById(int id);
        Category GetByIdWithProducts(int categoryId);
        Task<List<Category>> GetAll(); //asekroona cevirdik
        void Create(Category entity);
         Task<Category> CreateAsync(Category entity); //create metodunu burda da ekledik yeniden asekron olarak
        void Update(Category entity);
        void Delete(Category entity);
       void DeleteFromCategory(int productId,int categoryId);
    }
}