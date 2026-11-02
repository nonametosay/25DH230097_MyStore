using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _25DH230097_MyStore.Models.ViewModel
{
    public class ProfileVM
    {
        [Required(ErrorMessage = "Họ tên không được để trống")]
        [Display(Name = "Họ và Tên")]
        public string CustomerName { get; set; }
        [Required(ErrorMessage ="Số điện thoại không được để trống")]
        [Display(Name = "Số điện thoại")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Số điện thoại phải là 10 chữ số")]
        public string CustomerPhone { get; set; }

        [Required(ErrorMessage = "Làm ơn, email nè nhập hộ tui nha!")]
        [Display(Name = "Nhập email")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@gmail\.com$", ErrorMessage = "Email phải có định dạng @gmail.com")]
        [DataType(DataType.EmailAddress)]
        public string CustomerEmail { get; set; }
        [Required(ErrorMessage = "Rồi không nhập địa chỉ rồi sao giao hàng?")]
        [Display(Name = "Địa chỉ")]
        public string CustomerAddress { get; set; }
        [Required(ErrorMessage = "Buộc phải tên người dùng!")]
        [Display(Name = "Tên đăng nhập")]
        public string Username { get; set; }
    }
}