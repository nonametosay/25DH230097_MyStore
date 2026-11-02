using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList.Mvc;

namespace _25DH230097_MyStore.Models.ViewModel
{
    public class HomeProductVM
    {
        public string SearchTerm { get; set; }

        //Thuộc tính hỗ trợ phân trang
        public int PageNumber { get; set; } //Trang hiện tại
        public int PageSize { get; set; } // Số lượng sản phẩm trên 1 trang

        //danh sách sản phẩm nổi bật
        public List<Product> FeaturedProducts { get; set; }
        //danh sách sản phẩm mới đã phân trang 
        public PagedList.IPagedList<Product> NewProducts { get; set; }

    }
}