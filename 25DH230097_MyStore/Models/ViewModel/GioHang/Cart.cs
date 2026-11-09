using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _25DH230097_MyStore.Models.ViewModel.GioHang
{
    public class Cart
    {
        private List<CartItem> items = new List<CartItem>();// Danh sách các mặt hàng trong giỏ
        public IEnumerable<CartItem> Items => items;// Trả về danh sách các mặt hàng trong giỏ
        //Danh sách các sản phẩm trong giỏ hàng được nhóm theo CategoryName
        public List<IGrouping<string, CartItem>> GroupedItems => items.GroupBy(i => i.Category).ToList();
        //Các thuộc tính hỗ trợ phân trang
        public int PageNumber { get; set; }
        public int PageSize { get; set; } = 6;
        //Danh sách các sản phẩm cùng danh mục trong giỏ
        public  PagedList.IPagedList<Product> SimilarProduct { get; set; }

        //Thêm sản phẩm vào giỏ
        public void AddItem(int productId, string productImage, string productName, string productDescription, decimal unitPrice, int quantity, string category)
        {
            var existingItem = items.FirstOrDefault(i=>i.ProductID == productId);
            if (existingItem == null)
            {
                items.Add(new CartItem { ProductID = productId, ProductImage = productImage, ProductName = productName, ProductDescription = productDescription, UnitPrice = unitPrice, Quantity = quantity, Category = category });
            }
            else
            {
                existingItem.Quantity += quantity;
            }
        }

        //Xóa sản phẩm khỏi giỏ
        public void RemoveItem(int productId)
        {
            items.RemoveAll(i=>i.ProductID == productId);
        }
        //Tính tổng giá trị của giỏ
        public decimal TotalValue()
        {
            return items.Sum(i => i.TotalPrice);
        }

        //Làm trống giỏ hàng
        public void Clear()
        {
            items.Clear();
        }

        //Cập nhật số lượng sản phẩm đã chọn
        public void UpdateQuantity(int productId, int quantity)
        {
            var item = items.FirstOrDefault(i => i.ProductID == productId);
            if (item != null)
            {
                item.Quantity = quantity;
            }
        }
    }
}