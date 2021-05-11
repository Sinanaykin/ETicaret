using Microsoft.AspNetCore.Mvc;
using shopapp.business.Abstract;
using shopapp.entity;
using shopapp.webapi.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace shopapp.webapi.Controllers
{
    [ApiController] //bunu eklemeliyiz en başa
    //localhost:4200/api/products
    //localhost:4200/api/products/2
    [Route("api/[controller]")] //controller değişken burda controller ismine göre değişir
    public class ProductsController:ControllerBase //ControllerBase den türeticez (View desteği olmadan oluşturulan bir sınıf=ControllerBase)
    {
        private IProductService _productService;
      public ProductsController(IProductService productService)//ctor yaptık dısarıdan productService gelir
      {
          _productService=productService;
      }
        [HttpGet]  // /api/products dersek bu metod çalısır
        public async Task<IActionResult> GetProducts() //geri dönüş değeri string olan liste olarak dönen GetProducts metodu oluşturduk 
        {
           var products= await _productService.GetAll();//bütün ürünler gelir
           var productsDTO= new List<ProductDTO>(); //liste tanımladık döünüşü productDTO olan.DTO lar Model ler gibi düşünülebilir.
           foreach (var p in products)
           {
               productsDTO.Add(ProductToDTO(p)); //ProductToDTO DİYE METOD TANIMLADIK EN ALTTA ONU ÇAĞIRIYORUZ DİREK.ProductDTO daki ProductId ye database den gelen ProductId yi tanımlıyoruz bu metodta mesela
           }
            return Ok(productsDTO);//geriye 200 kodu ile birlikte productsDTO bilgileri döner
        }
        
        [HttpGet("{id}")] //api/products/3 yazarsak alttaki metod çalısıcak.İd parametresinide alıcak
        public async Task<IActionResult> GetProduct(int id) 
        {
            var p= await _productService.GetById(id);//id ye göre ürün gelir
            if (p==null)//ürün gelmezse
            {
                return NotFound(); //404 hatası gider
            }
            return Ok(ProductToDTO(p)); //200 kodu gider basarılı demek.ProductToDTO YU EN ALTTA METOD OLARAK TANIMLADIK ONU ÇAĞIRIYORUZ DİREK
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(Product entity)//dışarıdan product bilgisi alıcak
        {
            await _productService.CreateAsync(entity); //veritabanına eklicez  burda
            return CreatedAtAction(nameof(GetProduct),new {id=entity.ProductId},ProductToDTO(entity));//201 geriye döndürür yeni bir kaynak oluştu demek.
            //createdATACTİON BİZE 201 İ GERİYE DÖNDÜRÜR BUNLA BİRLİKTE oluşan entity bilgisi geri döner,nameof(GetProduct) ile detay bilgilerini görüntüleriz ismini ,sonra id bilgisini göndeririz,3. olarak ProductToDTO üzerinden entity ekleriz  veritananına eklenen kaydı kullanıcı görmüş olur
          //nameof(GetProduct) demek yerine "GetProduct" da yazabiliriz


        }

        [HttpPut("{id}")] //güncelleme post da olur ama put daha iyi dışarıdan id bilgisi alıcaz onu belirttik
        public async Task<IActionResult> UpdateProduct(int id,Product entity) //dışarıdan id ve product bilgisi alıcak
        {
            if (id!=entity.ProductId)//eğer id eşit değildir gönderdiğimiz entity nin productId sine
            {
                return BadRequest();//404 lü bir hata mesajı döndürürüz
            }
            var product=await _productService.GetById(id); //HATA YOKSA PRODUCT BİLGİSİNİ VERİTABANINDAN ALICAZ İD YE GÖRE
            if (product==null)//product null ise
            {
                return NotFound();//bu hatayı döndür 404
            }
            
            await _productService.UpdateAsync(product,entity);//UpdateASYNC METODU 2 DEĞER BEKLİYO BİRİ DATABASEDEN GELEN BİLGİLER DİĞERİ POSTMANDEN YOLLAYACAĞIMIZ BİLGİLER. 2 değer gönderiliyor.product veritabanından aldığımız değer ,entity ise  postman den gönderdiğimiz değer günceller burda veritanındakini
            return NoContent();//başarılı bir sonuc döndürür 200 lü değer
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id) //dısarıdan id alıcaz
        {
            var product=await _productService.GetById(id);//id ye göre ürünü alıcaz
             if (product==null)//ürün gelmezse
            {
                return NotFound(); //404 hatası gider
            }
            await _productService.DeleteAsync(product); //id si alınan ürünü silicez databse den
            return NoContent();

        }
        private static ProductDTO ProductToDTO(Product p)
        {
            return new ProductDTO{  //ProductToDTO DİYE METOD TANIMLADIK .ProductDTO daki ProductId ye database den gelen ProductId yi tanımlıyoruz bu metodta mesela
                   ProductId=p.ProductId,
                   Name=p.Name,
                   Url=p.Url,
                   Description=p.Description,
                   Price=p.Price,
                   ImageUrl=p.ImageUrl

               };
        }
    }
}