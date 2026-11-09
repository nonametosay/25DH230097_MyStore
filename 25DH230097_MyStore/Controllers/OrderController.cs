using _25DH230097_MyStore.Models;
using _25DH230097_MyStore.Models.ViewModel.DatHang;
using _25DH230097_MyStore.Models.ViewModel.GioHang;
using System;
using System.Collections.Generic;
using System.Data.Entity; // THÊM: để dùng Include("OrderDetails") trong OrderSuccess
using System.Linq;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;

namespace _25DH230097_MyStore.Controllers
{
    public class OrderController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();
        // GET: Order
        public ActionResult Index()
        {
            return View();
        }

        //GET: Order/CheckOut
        [Authorize]
        public ActionResult Checkout()
        {
            //Kiểm tra giỏ hàng trong session
            //Nếu giỏ hàng rỗng hoặc không có sản phẩm thì chuyển về trang chủ
            // SỬA: Session lưu object Cart (do CartService), không phải List<CartItem> nên phải lấy Cart ra rồi lấy Items
            var cart = (Session["Cart"] as Cart)?.Items.ToList();
            if (cart == null || !cart.Any())
            {
                // THÊM: gửi thông báo về trang chủ bằng TempData (ViewBag sẽ mất khi redirect)
                TempData["ErrorMessage"] = "Giỏ hàng của bạn đang trống, vui lòng thêm sản phẩm trước khi đặt hàng!";
                return RedirectToAction("Index", "Home");
            }

            //Xác thực người dùng đã đăng nhập chưa, nếu chưa thì chuyển hướng tới trang Đăng nhập
            var user = db.Users.SingleOrDefault(u => u.Username == User.Identity.Name);
            if (user == null) { return RedirectToAction("Login", "Account"); }
            //Lấy thông tin khách hàng từ CSDL, nếu chưa có thì chuyển hướng tới trang Đăng nhập
            //Nếu có rồi thì lấy địa chỉ của kahcsh hàng và gán vào ShippingAddress của CheckoutVM
            var customer = db.Customers.SingleOrDefault(c => c.Username == user.Username);
            if (customer == null) { return RedirectToAction("Login", "Account"); }

            var model = new CheckoutVM //Tạo dữ liệu hiển thị cho CheckoutVM
            {
                CartItems = cart, //Lấy danh sách các sản phẩm trong giỏ hàng
                TotalAmount = cart.Sum(item => item.TotalPrice),//Tổng giá trị của các mặt hàng trong giỏ
                OrderDate = DateTime.Now, //Mặc định lấy bằng thời điểm đặt hàng
                ShippingAddress = customer.CustomerAddress,//Lấy địa chỉ mặc định từ bảng Customer
                CustomerID = customer.CustomerID, //Lấy mã khách hàng từ bảng Customer
                Username = customer.Username, //Lấy tên đăng nhập từ bảng customer
                DeliveryMethod = "Giao hàng nhanh", // THÊM: chọn sẵn radio phương thức giao hàng (thay cho "checked" bị lỗi trong view)
                PaymentMethod = "Tiền mặt" // THÊM: chọn sẵn radio phương thức thanh toán (thay cho "checked" bị lỗi trong view)


            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Checkout(CheckoutVM model)
        {
            // THÊM: đưa việc lấy giỏ hàng ra ngoài if để dùng được cả ở cuối hàm khi form bị lỗi
            // SỬA: Session lưu object Cart, không phải List<CartItem>
            var cart = (Session["Cart"] as Cart)?.Items.ToList();

            if (ModelState.IsValid)
            {
                //Nếu giỏ hàng rỗng sẽ điều hướng tới trang home
                // SỬA: kiểm tra thêm !cart.Any() cho giống action GET
                if (cart == null || !cart.Any())
                {
                    // THÊM: gửi thông báo về trang chủ bằng TempData (ViewBag sẽ mất khi redirect)
                    TempData["ErrorMessage"] = "Giỏ hàng của bạn đang trống, vui lòng thêm sản phẩm trước khi đặt hàng!";
                    return RedirectToAction("Index", "Home");
                }

                // SỬA: lấy user từ tài khoản đang đăng nhập, vì form không gửi Username lên nên model.Username bị null
                var user = db.Users.SingleOrDefault(u => u.Username == User.Identity.Name);
                //Nếu người dùng chưa đăng nhập sẽ điều hướng tới trang Login
                if (user == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                //Nếu khách hàng không khớp với tên đăng nhập, sẽ điều hướng tới trang Login
                var customer = db.Customers.SingleOrDefault(c => c.Username == user.Username);
                if (customer == null) { return RedirectToAction("Login", "Account"); }

                //Nếu người dùng chọn thanh toán bằng Paypal, sẽ điều hướng tới trang PaymentWithPaypal
                if (model.PaymentMethod == "Paypal")
                {
                    return RedirectToAction("PaymentWithPaypal", "PayPal", model);
                }
                //Thiết lập paymentStatus dựa trên Payment Method
                string paymentStatus = "Chưa thanh toán";
                switch (model.PaymentMethod)
                {
                    case "Tiền mặt": paymentStatus = "Thanh toán tiền mặt"; break;
                    //case "Paypal": paymentStatus = "Thanh toán bằng Paypal";break;
                    case "Mua trước trả sau": paymentStatus = "Trả góp"; break;
                    default: paymentStatus = "Chưa thanh toán"; break;
                }

                //Tạo đơn hàng và chi tiết đơn hàng liên quan
                var order = new Order
                {
                    CustomerID = customer.CustomerID,
                    OrderDate = DateTime.Now, // SỬA: form không gửi OrderDate lên nên model.OrderDate = 01/01/0001, gây lỗi SQL
                    TotalAmount = cart.Sum(item => item.TotalPrice), // SỬA: tính lại từ giỏ hàng, vì form không gửi giá trị này lên
                    PaymentStatus = paymentStatus,
                    PaymentMethod = model.PaymentMethod,
                    DeliveryMethod = model.DeliveryMethod,
                    ShippingAddress = model.ShippingAddress,
                    OrderDetails = cart.Select(item => new OrderDetail
                    {
                        ProductID = item.ProductID,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                        TotalPrice = item.TotalPrice
                    }).ToList()
                };
                //Lưu đơn hàng vào CSDL
                db.Orders.Add(order);
                db.SaveChanges();
                //Xóa giỏ hàng sau khi đặt hàng thành công
                Session["Cart"] = null;
                //Điều hướng tới trang xác nhạn đơn hàng
                return RedirectToAction("OrderSuccess", new { id = order.OrderID });
            }
            model.CartItems = cart; // THÊM: nạp lại danh sách sản phẩm, vì form không gửi CartItems lên, nếu thiếu thì view bị lỗi khi hiển thị lại
            return View(model);
        }

        // THÊM: action OrderSuccess (slide 46), nếu thiếu thì đặt hàng xong sẽ bị 404
        public ActionResult OrderSuccess(int id)
        {
            var order = db.Orders.Include("OrderDetails").SingleOrDefault(o => o.OrderID == id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // THÊM: trang Đơn hàng của tôi (slide 48), chỉ hiện đơn của khách đang đăng nhập
        [Authorize]
        public ActionResult MyOrder(string searchTerm, string status)
        {
            var customer = db.Customers.SingleOrDefault(c => c.Username == User.Identity.Name);
            if (customer == null) { return RedirectToAction("Login", "Account"); }

            // Lấy đơn của khách này, kèm chi tiết đơn và sản phẩm để hiện tên, ảnh
            var orders = db.Orders.Include("OrderDetail.Product")
                                  .Where(o => o.CustomerID == customer.CustomerID);

            // Lọc theo tab trạng thái (để trống = Tất cả)
            if (!string.IsNullOrEmpty(status))
            {
                orders = orders.Where(o => o.PaymentStatus == status);
            }

            // Tìm theo mã đơn hàng hoặc tên sản phẩm
            if (!string.IsNullOrEmpty(searchTerm))
            {
                int orderId;
                bool isNumber = int.TryParse(searchTerm, out orderId);
                orders = orders.Where(o => (isNumber && o.OrderID == orderId)
                                           || o.OrderDetails.Any(d => d.Product.ProductName.Contains(searchTerm)));
            }

            // Giữ lại giá trị để view tô sáng tab và điền lại ô tìm kiếm
            ViewBag.Status = status;
            ViewBag.SearchTerm = searchTerm;

            return View(orders.OrderByDescending(o => o.OrderDate).ToList());
        }

    }
}