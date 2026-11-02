using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace _25DH230097_MyStore.Models.ViewModel
{
    public class LoginVM
    {
        [Required(ErrorMessage ="Vui lòng nhập tên đăng nhập")]
        [Display(Name ="Tên đăng nhập")]
        public string Username { get; set; }
        [Required(ErrorMessage ="Vui lòng nhập mật khẩu")]
        [DataType(DataType.Password)]
        [Display(Name ="Mật khẩu")]
        public string Password { get; set; }
    }
}