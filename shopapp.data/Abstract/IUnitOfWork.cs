using System;
using System.Threading.Tasks;

namespace shopapp.data.Abstract
{
    public interface IUnitOfWork: IDisposable //IDisposable dan türücek
    {
       //tanımlamıs oldugumuz bütün repository sınıflarını buraya alıcaz
         ICartRepository  Carts {get;}
         ICategoryRepository Categories {get;}
         IOrderRepository Orders {get;}
         IProductRepository Products {get;}
         void Save(); //geri dönüş değeri olmayan save metodunuda eklicez
          Task<int> SaveAsync(); //web api için asekron halini de ekledik save metodu için

    }
}