using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using _25DH230097_MyStore.Models;

namespace _25DH230097_MyStore.Areas.Admin.Controllers
{
    public class OrdersController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();

        // GET: Admin/Orders
        public ActionResult Index()
        {
            // Lấy danh sách đơn hàng, sắp xếp mới nhất lên đầu
            var orders = db.Orders.Include(o => o.Customer).OrderByDescending(o => o.OrderID);
            return View(orders.ToList());
        }

        // GET: Admin/Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Lấy Order và thông tin Customer
            Order order = db.Orders.Include(o => o.Customer).SingleOrDefault(o => o.OrderID == id);
            if (order == null)
            {
                return HttpNotFound();
            }

            // Truyền danh sách OrderDetails qua ViewBag để hiển thị Hóa đơn
            ViewBag.OrderDetails = db.OrderDetails.Include(od => od.Product)
                                                  .Where(od => od.OrderID == id).ToList();

            return View(order);
        }

        // GET: Admin/Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Include(o => o.Customer).SingleOrDefault(o => o.OrderID == id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Admin/Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            if (order != null)
            {
                // 1. Tìm tất cả OrderDetail của đơn hàng này
                var orderDetails = db.OrderDetails.Where(od => od.OrderID == id).ToList();

                // 2. Xóa các OrderDetail trước (tránh lỗi khóa ngoại)
                if (orderDetails.Any())
                {
                    db.OrderDetails.RemoveRange(orderDetails);
                }

                // 3. Xóa Order
                db.Orders.Remove(order);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}