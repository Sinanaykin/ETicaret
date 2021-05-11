using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using shopapp.data.Abstract;

namespace shopapp.data.Concrete.EfCore
{
    public class EfCoreGenericRepository<TEntity> : IRepository<TEntity> //base class bu

       where TEntity : class
    {
        protected readonly DbContext context; //Base class olduğu için yani genel olduğu için  DbContexti aldık
        public EfCoreGenericRepository(DbContext ctx) //ctor yaptık burda.Burda context i alıyoruz diğer Ef lerde de base 'e ekliyoruz contexti direk kalıtım yapıyoruz yani.
        {
            context=ctx;
        }

        
        public void Create(TEntity entity)
        {
           
                context.Set<TEntity>().Add(entity);
               
        }

        public async Task CreateAsync(TEntity entity)
        {
            await context.Set<TEntity>().AddAsync(entity);
        }

        public void Delete(TEntity entity)
        {
            context.Set<TEntity>().Remove(entity);
              
            
        }

        public async Task<List<TEntity>> GetAll() //asekrona cevirmiştik metodu IProductService de burda da asekron yaptık
        {
            
                return await context.Set<TEntity>().ToListAsync();
            
        }

        public async Task<TEntity> GetById(int id)
        {
            
                return await context.Set<TEntity>().FindAsync(id);
            
        }

        public virtual void Update(TEntity entity)
        {
            
                context.Entry(entity).State=EntityState.Modified;
                
            
        }
    }
}