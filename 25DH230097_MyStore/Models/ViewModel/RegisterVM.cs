using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace _25DH230097_MyStore.Models.ViewModel
{
    public class RegisterVM //Lưu thông tin form đăng kí tài khoản khách hàng
    {
        [Required(ErrorMessage ="Buộc phải tên người dùng!")]
        [Display(Name ="Tên đăng nhập")]
        public string Username { get; set; }

        [Required(ErrorMessage="Buộc phải nhập mật khẩu!")]
        [DataType(DataType.Password)]
        [Display(Name ="Mật khẩu")]
        public string Password { get; set; }

        [Required(ErrorMessage ="Phải nhập mật khẩu xác nhận!")]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage ="Hai mật khẩu không trùng!")]
        [Display(Name ="Xác nhận mật khẩu")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage ="Bạn quên nhập tên rồi kìa")]
        [Display(Name ="Họ và tên")]
        public string CustomerName { get; set; }

        [Required(ErrorMessage ="Ủa bạn ơi, số điện thoại đâu?")]
        [Display(Name ="Số điện thoại")]
        [RegularExpression(@"^[0-9]{10,12}$",ErrorMessage = "Số điện thoại 10-12 số")]
        [DataType(DataType.PhoneNumber)]
        public string CustomerPhone { get; set; }

        [Required(ErrorMessage ="Làm ơn, email nè nhập hộ tui nha!")]
        [Display(Name ="Nhập email")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@gmail\.com$", ErrorMessage = "Email phải có định dạng @gmail.com")]
        [DataType(DataType.EmailAddress)]
        public string CustomerEmail { get; set; }

        [Required(ErrorMessage ="Rồi không nhập địa chỉ rồi sao giao hàng?")]
        [Display(Name ="Địa chỉ")]
        public string CustomerAddress { get; set; }
        
    }
}