using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _25DH230097_MyStore.Models.ViewModel
{
    public class ProductDetailsVM
    {
        public Product product { get; set; }
        public int quantity { get; set; }
        //Tính giá trị tạm thời quantity * product.ProductPrice
        public decimal estimatedValue => quantity * product.ProductPrice;

        //Các thuộc tính hỗ trợ phân trang
        public int PageNumber { get; set; }//Trang hiện tại
        public int PageSize { get; set; } = 3;//Số lượng sản phẩm trên mỗi trang

        //danh sách 8 sản phẩm cùng danh mục
        public PagedList.IPagedList<Product> RelatedProducts { get; set; }
        //public List<Product> RelatedProducts { get; set; }
        //Danh sách 8 sản phẩm bán chạy nhất
        public PagedList.IPagedList<Product> TopProducts { get; set; }
    }
}