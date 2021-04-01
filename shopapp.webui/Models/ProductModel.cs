using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using shopapp.entity;


namespace shopapp.webui.Models
{
    public class ProductModel
    {
        public int ProductId { get; set; }

        //  [Display(Name="Name",Prompt="Enter product name")]
        //  [Required(ErrorMessage="Name bilgisi zorunlu bir alandır.")]
        //  [StringLength(60,ErrorMessage="Name bilgisi maximum 60 karakter uzunluğundadır.")]
        public string Name { get; set; }

        [Required(ErrorMessage="Url bilgisi zorunlu bir alandır.")]
        public string Url { get; set; }

        // [Required(ErrorMessage="Price bilgisi zorunlu bir alandır.")]
        // [Range(1,30000,ErrorMessage="Price bilgisi için 1 ile 30000 arasında değer girmelisiniz.")]
        public double? Price { get; set; }

        [StringLength(100,ErrorMessage="Description bilgisi maximum 100 karakter uzunluğundadır.")]
        [Required(ErrorMessage="Description bilgisi zorunlu bir alandır.")]
        public string Description { get; set; }

        // [Required(ErrorMessage="ImageUrl bilgisi zorunlu bir alandır.")]
        public string ImageUrl { get; set; }
        

        public bool IsApproved { get; set; }

        public bool IsHome { get; set; }

        public List<Category> SelectedCategories { get; set; }
    }
}