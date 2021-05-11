using System.Collections.Generic;
using System.Threading.Tasks;
using shopapp.entity;

namespace shopapp.business.Abstract
{
    public interface IProductService:IValidator<Product>
    {

        Task<Product> GetById(int id);
        Product GetByIdWithCategories(int id);
         Product GetProductDetails(string url);
        List<Product> GetProductsByCategory(string name,int page,int pageSize);
        
        List<Product> GetHomePageProducts();

        List<Product> GetSearchResult(string searchString);
        Task<List<Product>> GetAll(); //asekrona cevirdik
        bool Create(Product entity);
        Task<Product> CreateAsync(Product entity); //create metodunu burda da ekledik yeniden asekron olarak
        void Update(Product entity);
        Task UpdateAsync(Product entityToUpdate,Product entity);//ilki veritanından seçiceğimiz entity 2.si yeni gelicek entity.Asekron bir update metodu oluşturduk
        void Delete(Product entity);
        Task DeleteAsync(Product entity);//asekron yaptık
         bool Update(Product entity,int[] categoryIds);
        int GetCountByCategory(string category);
    }
}