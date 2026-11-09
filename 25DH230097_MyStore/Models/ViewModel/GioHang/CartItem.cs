using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _25DH230097_MyStore.Models.ViewModel.GioHang
{
    public class CartItem// Lưu thông tin mỗi mặt hàng có trong giỏ
    {
        public int ProductID { get; set; }
        public string ProductImage { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        //public string Category { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public string Category { get; set; }
        public decimal TotalPrice => UnitPrice * Quantity;



    }
}