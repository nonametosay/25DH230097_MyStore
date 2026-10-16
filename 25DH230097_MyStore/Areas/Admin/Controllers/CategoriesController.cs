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
    public class CategoriesController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities(); //khai báo câu lệnh này để truy vấn vào csdl

        // GET: Admin/Categories
        //GET: lấy dữ liệu từ bảng Category trong DB để hiển thị lên
        public ActionResult Index()
        {
            return View(db.Categories.ToList());
        }

        // GET: Admin/Categories/Details/5
        // Lấy chi tiết một bản ghi có CategoryID = id
        public ActionResult Details(int? id) //Ký hiệu "?" có thể cho truyền ID rỗng, ví dụ: nếu ko truyền Id thì ở dưới đã có code kiểm tra id==null
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);//Mã lỗi 400: thiếu giá trị truyền vào
            }
            Category category = db.Categories.Find(id);
            if (category == null) //Không tìm thấy bản ghi với id tương ứng.
            {
                return HttpNotFound(); //mã lỗi 404
            }
            return View(category);
        }

        // GET: Admin/Categories/Create
        //Lấy 1 bản ghi từ DB và hiển thị lên form Create
        //[HttpGet] phương thức mặc định nên không cần khai báo
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Categories/Create
        // POST: Lưu dữ liệu nhập vào từ form Create vào database.
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken] //ví dụ lưu về db. Ta nhấn gửi cả chục lần mà ko có cái ActionVerb này thì khả năng là Database lưu cả chục bản ghi.
        public ActionResult Create([Bind(Include = "CategoryID,CategoryName")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(category);
        }

        // GET: Admin/Categories/Edit/5
        //GET: lấy dữ liệu của một danh mục đã có sao cho CategoryID = id
        public ActionResult Edit(int? id)
        {
            return Details(id);
           // Phần code này y chang phần Detail nên ko cần phải ghi lại
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //Category category = db.Categories.Find(id);
            //if (category == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(category);
        }

        // POST: Admin/Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CategoryID,CategoryName")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // GET: Admin/Categories/Delete/5
        public ActionResult Delete(int? id)
        {
            return Details(id);
            //code kiểm tra id có tồn tại hoặc người dùng có nhập id hay không, y hệt Details 
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //Category category = db.Categories.Find(id);
            //if (category == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(category);
        }

        // POST: Admin/Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = db.Categories.Find(id);
            db.Categories.Remove(category);
            db.SaveChanges();
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
