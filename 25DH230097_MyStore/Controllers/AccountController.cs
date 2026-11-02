using _25DH230097_MyStore.Models;
using _25DH230097_MyStore.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace _25DH230097_MyStore.Controllers
{
    public class AccountController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();
        //GET: Account/Register
        public ActionResult Register()
        {
            return View();
        }
        //POST: Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken] //Chống spam tài khoản hoặc ép đăng kí
        public ActionResult Register(RegisterVM model)
        {
            if (ModelState.IsValid) //Kiểm tra liệu dữ liệu nhập có thỏa điều kiện
            {
                //Kiểm tra xem tên đăng nhập đã tồn tại chưa
                var existingUser = db.Users.SingleOrDefault(u => u.Username == model.Username);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Username", "Tên đăng nhập này đã tồn tại");
                    return View(model);
                }
                //Nếu chưa tồn tại thì tạo bản ghi thông tin tài khản trong bảng User
                var user = new User
                {
                    Username = model.Username,
                    Password = model.Password, //Lưu ý: Nên mã hóa mật khẩu trước khi lưu
                    UserRole="C"
                };
                db.Users.Add(user);

                var customer = new Customer
                {
                    CustomerName = model.CustomerName,
                    CustomerEmail = model.CustomerEmail,
                    CustomerPhone = model.CustomerPhone,
                    CustomerAddress = model.CustomerAddress,
                    Username = model.Username
                };
                db.Customers.Add(customer);
                //Lưu tt khách hàng và tài khoản vào csdl
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View(model); //Nếu dữ liệu không thỏa điều kiện, thì sẽ cho người dùng nhập lại
        }

        //GET: Account/Login
        public ActionResult Login()
        {
            if (Session["Username"] != null) //Nếu người dùng đã đăng nhập thì ko cho vào trang login nữa mà chuyển hướng sang trang profile
            {
                return RedirectToAction("UserProfile", "Account");
            }
            return View();
        }
        //POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginVM model)
        {
           
            if (ModelState.IsValid)
            {
                var user = db.Users.SingleOrDefault(u => u.Username == model.Username && u.Password == model.Password && u.UserRole == "C");
                if (user!= null)
                {
                    //Lưu trạng thái đăng nhập vào session
                    Session["Username"] = user.Username;
                    Session["UserRole"] = user.UserRole;
                    //Lưu thông tin xác thực người dùng vào cookie
                    FormsAuthentication.SetAuthCookie(user.Username, false);
                    return RedirectToAction("UserProfile", "Account");
                }
                else
                {
                    ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
                }
            }
            return View(model);
        }
        //Get: Account/Profile
        public ActionResult UserProfile()
        {
            //Kiểm tra xem đã đăng nhập chưa( dựa vào session)
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            string currentUser= Session["Username"].ToString();
            //Lấy dữ liệu từ User và Customer dựa vào tên đăng nhập
            var user = db.Users.SingleOrDefault(u=> u.Username == currentUser);
            var customer = db.Customers.SingleOrDefault(c => c.Username == currentUser);
            if (user == null || customer == null)
            {
                // Gửi thông báo lỗi ra View
                ViewBag.ErrorMessage = "Không tìm thấy thông tin tài khoản của bạn. Vui lòng liên hệ Admin!";
                return View(); // Trả về trang Profile nhưng kèm lỗi
            }

            //Nếu tồn tại tài khoản thì đổ vào profileVM để hiển thị thông tin
            var profile = new ProfileVM
            {
                CustomerName = customer.CustomerName,
                CustomerPhone = customer.CustomerPhone,
                CustomerEmail = customer.CustomerEmail,
                CustomerAddress = customer.CustomerAddress,
                Username = user.Username
            };
            return View(profile);
        }

        //POST: Account/Profile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserProfile(ProfileVM model)
        {
            if (ModelState.IsValid)
            {
                string currentUser = Session["Username"].ToString();
                var customer = db.Customers.SingleOrDefault(c => c.Username == currentUser);
                if (customer == null)
                {
                    ViewBag.ErrorMessage = "Không tìm thấy thông tin tài khoản của bạn. Vui lòng liên hệ Admin!";
                    return View(model);
                }
                //Cập nhật thông tin khách hàng
                customer.CustomerName = model.CustomerName;
                customer.CustomerPhone = model.CustomerPhone;
                customer.CustomerEmail = model.CustomerEmail;
                customer.CustomerAddress = model.CustomerAddress;
                db.SaveChanges();
                ViewBag.SuccessMessage = "Cập nhật thông tin thành công!";
                
            }
            return View(model);
        }

        //GET: Account/ChangePassword
        public ActionResult ChangePassword()
        {
            //Kiểm tra xem đã đăng nhập chưa( dựa vào session)
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }
        //POST: Account/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordVM model)
        {
            if (ModelState.IsValid)
            {
                string currentUser = Session["Username"].ToString();
                var user = db.Users.SingleOrDefault(u => u.Username == currentUser);
                if (user != null)
                {
                    if (user.Password.Trim() != model.OldPassword)
                    {
                        ModelState.AddModelError("OldPassword", "Mật khẩu cũ không đúng!");
                        return View(model);
                    }
                    user.Password = model.NewPassword; //Cập nhật mật khẩu mới
                    db.SaveChanges();
                    ViewBag.SuccessMessage = "Đổi mật khẩu thành công!";
                    return View(model);
                }
            }
            return View(model);
        }
        // GET: Account/Logout không gắn post vì logout ko cần dữ liệu từ form và bấm vào là xóa ngay
        public ActionResult Logout()
        {
            // 1. Xóa toàn bộ Session
            Session.Clear();
            Session.Abandon();

            // 2. Xóa Cookie xác thực (nếu bạn có dùng FormsAuthentication)
            FormsAuthentication.SignOut();

            // 3. Chuyển hướng về trang chủ
            return RedirectToAction("Index", "Home");
        }
    }
}