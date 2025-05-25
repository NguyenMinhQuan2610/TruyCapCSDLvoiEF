using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebBanHang.Models;
namespace WebBanHang.Controllers
{

    public class CategoryController : Controller
    {
        private ApplicationDbContext _db;
        private IWebHostEnvironment _hosting;
        public CategoryController(ApplicationDbContext db, IWebHostEnvironment hosting)
        {
            _db = db;
            _hosting = hosting;
        }

        #region Xem_DSSP
        public IActionResult Index()
        {

            //Đọc tất cả sản phẩm trong CSDLthông qua dbcontext
            //var dsSanPham = _db.Products.Where(x=>x.Price>400).OrderBy(p=>p.Price).ToList();//Linq (method syntax)
            //var dsSanPham = (from x in _db.Products where x.Price>400 select x).ToList();//Linq ( query syntax)
            var dsSanPham = _db.Categoríe.ToList();

            return View(dsSanPham);
        }
        #endregion
        //Xử lý xóa sản phẩm
        #region XuLy_Xoa
        public IActionResult Delete(int id)
        {
            //1.Truy vấn sản phẩm cần xóa trong CSDL

            var sp = _db.Categoríe.Find(id); //=> Truy vấn theo khóa chính
            //4.Điều hướng người dùng về lại action index
            return View(sp);

        }
        //Thông báo xác nhận 
        public IActionResult DeleteConfirm(int id)
        {
            //1.Truy vấn sản phẩm cần xóa trong CSDL
            var sp = _db.Categoríe.Find(id); //=> Truy vấn theo khóa chính
                                              //Cách 2
            if (_db.Products.Where(x => x.CategoryId == id).ToList().Count() > 0)                        //var sp=_db.Products.Where(x=>x.Id==id).FirstOrDefault();
            {
                TempData["error"] = "Không thể xóa danh mục vì vẫn còn sản phẩm thuộc danh mục này.";
                return RedirectToAction("Index");
            }
            ;
            //2.Thực hiện xóa sản phẩm
            _db.Categoríe.Remove(sp);
            _db.SaveChanges();
            TempData["success"] = "Xóa sản phẩm thành công";
            //3.Thông báo kết quả thao tác
            //4.Điều hướng người dùng về lại action index
            return RedirectToAction("Index");

        }
        #endregion
        #region XuLy_ThemSp
        [HttpPost]
        public IActionResult Add(Category product)
        {

            _db.Categoríe.Add(product);
            _db.SaveChanges();
            TempData["success"] = "Thêm sản phẩm thành công";

            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Add()
        {

            return View();
        }
        #endregion

        #region XuLy_SuaSP
        [HttpPost]
        public IActionResult Update(Category product)
        {
            //B1: Truy vấn sản phẩm cần cập nhật trong CSDL
            var OldProduct = _db.Categoríe.Find(product.Id);

            //cập nhật product vào table Product
            OldProduct.Name = product.Name;
            OldProduct.DisplayOrder = product.DisplayOrder;

            _db.SaveChanges();
            TempData["success"] = "Cập nhật sản phẩm thành công";
            return RedirectToAction("Index");

        }



        //Hiển thị form cập nhật sản phẩm
        public IActionResult Update(int id)
        {
            var sp = _db.Categoríe.Find(id);

            return View(sp);
        }
        #endregion
    }
}