using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList.Mvc;
namespace _25DH230097_MyStore.Models.ViewModel
{
    //Tạo ViewModel để truyền dữ liệu tìm kiếm      
    public class SearchProductVM
    {
        //tiêu chí theo tên, mô tả hoặc danh mục
        public string SearchTerm { get; set; }
        
        //Các tiêu chí theo giá
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        //Thứ tự sắp xếp
        public string SortOrder { get; set; }
        //Các thuộc tính hỗ trợ phân trang
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        //Danh sách sản phẩm thoải điều kiện tìm kiếm
        public PagedList.IPagedList<Product> Products { get; set; }
    }
}