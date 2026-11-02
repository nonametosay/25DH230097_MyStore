using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _25DH230097_MyStore.Models.ViewModel
{
    public class ChangePasswordVM
    {
        [Required(ErrorMessage = "Buộc phải nhập mật khẩu cũ!")]
        [DataType(DataType.Password)]
        [Display(Name ="Mật khẩu cũ")]
        public string OldPassword { get; set; }
        [Required(ErrorMessage = "Buộc phải nhập mật khẩu mới!")]
        [DataType(DataType.Password)]   
        [Display(Name ="Mật khẩu mới")]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "Buộc phải xác nhận mật khẩu mới!")]
        [DataType(DataType.Password)]
        [Display(Name ="Xác nhận mật khẩu mới")]
        [Compare("NewPassword", ErrorMessage = "Mật khẩu xác nhận không khớp!")]
        public string ConfirmNewPassword { get; set; }  
    }
}