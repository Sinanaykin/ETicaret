using System.Collections.Generic;
using System.Linq;
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

        public void Delete(TEntity entity)
        {
            context.Set<TEntity>().Remove(entity);
              
            
        }

        public List<TEntity> GetAll()
        {
            
                return context.Set<TEntity>().ToList();
            
        }

        public TEntity GetById(int id)
        {
            
                return context.Set<TEntity>().Find(id);
            
        }

        public virtual void Update(TEntity entity)
        {
            
                context.Entry(entity).State=EntityState.Modified;
                
            
        }
    }
}