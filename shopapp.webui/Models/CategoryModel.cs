using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using shopapp.entity;


namespace shopapp.webui.Models
{
    public class CategoryModel
    {
      public int CategoryId { get; set; }

      [Required(ErrorMessage="Name bilgisi zorunlu bir alandır.")]
      [StringLength(60,ErrorMessage="Name bilgisi maximum 100 karakter uzunluğundadır.")]
      public string Name { get; set; }
      
      [Required(ErrorMessage="Url bilgisi zorunlu bir alandır.")]
      public string Url { get; set; }

      public List<Product> Products { get; set; }
   
    }
}