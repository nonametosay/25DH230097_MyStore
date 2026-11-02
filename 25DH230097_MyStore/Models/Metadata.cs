using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace _25DH230097_MyStore.Models
{
    public class UserMetadata
    {
        [Required(ErrorMessage ="Bắt buộc phải nhập tên người dùng! ")]
        [StringLength(30,MinimumLength =5,ErrorMessage ="Tối thiểu 5 kí tự, tối đa 30 kí tự")]
        public string Username { get; set; }

        [Required(ErrorMessage ="Buộc phải nhập mật khẩu")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string UserRole { get; set; }
    }

    public class ProductMetadata
    {
        [HiddenInput]
        public int ProductID { get; set; }
        [HiddenInput]
        public int CategoryID { get; set; }
        [Required(ErrorMessage ="Nhập tên sản phẩm")]
        [StringLength(50,MinimumLength =5)]
        public string ProductName { get; set; }
        [Required(ErrorMessage ="Nhập giá!")]
        public decimal ProductPrice { get; set; }
        public string ProductImage { get; set; }
        
        public string ProductDescription { get; set; }
    }

    public class CategoryMetadata
    {
        [HiddenInput]
        public int CategoryID { get; set; }
        [Required(ErrorMessage ="Nhập tên danh mục")]
        [StringLength(50, MinimumLength = 5)]
        public string CategoryName { get; set; }
    }
}