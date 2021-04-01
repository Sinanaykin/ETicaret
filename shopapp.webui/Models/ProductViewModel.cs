using System.Collections.Generic;
using shopapp.entity;
using System;

namespace shopapp.webui.Models
{
public class PageInfo
{
    public int TotalItems { get; set; }
    public int ItemPerPage { get; set; }
    public int CurrentPage { get; set; }
    public string CurrentCategory { get; set; }

   public int TotalPages()
    {
        return (int)Math.Ceiling((decimal)TotalItems/ItemPerPage);
    }
}

public class ProductListViewModel//ctrl nokta yap
{
    public PageInfo PageInfo { get; set; }
   
    public List<Product> Products { get; set; }//ctrl nokta yap
}
}