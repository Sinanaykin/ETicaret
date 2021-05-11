using System.Collections.Generic;
using System.Threading.Tasks;

namespace shopapp.data.Abstract
{
    public interface IRepository<T>
    {
        Task<T> GetById(int id);
        Task<List<T>> GetAll();//asekrona cevirdik metodu
        void Create(T entity);
         Task CreateAsync(T entity);//bir üstteki create i heryerde değiştirip asekron yapmak yerine buraya direk asekron olan create metodu ekledik
        void Update(T entity);
        void Delete(T entity);
    }
}