using Microsoft.AspNetCore.Mvc;
using WebBanHang.Models;
using WebBanHang.Areas.Customer.Models;

namespace WebBanHang.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ProductController : Controller
    {
        private ApplicationDbContext _db;
        public ProductController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index(int id = 1)
        {
            //Lấy dữ liệu từ CSDL
            var dsLoai = _db.Categorise.OrderBy(x => x.DisplayOrder)
                .Select(c => new CategoryViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    TotalProduct = _db.Products.Where(p => p.CategoryId == c.Id).Count()
                })
                .ToList();
            var dsSanPham = _db.Products
        .Where(x => x.CategoryId == id)
        .OrderBy(x => x.Price) 
        .ToList();

            //Truyền qua View
            ViewBag.dsLoai = dsLoai;
            ViewBag.Category_Name = _db.Categorise.Find(id).Name;
            return View(dsSanPham);
        }

        // Trả về view không dùng layout
        public IActionResult LoadProduct_Partial(int id = 1)
        {
            //Lấy dữ liệu từ CSDL
            var dsLoai = _db.Categorise.OrderBy(x => x.DisplayOrder).ToList();
            var dsSanPham = _db.Products.Where(x => x.CategoryId == id).ToList();

            //Truyền qua View
            ViewBag.dsLoai = dsLoai;
            ViewBag.Category_Name = _db.Categorise.Find(id).Name;
            return PartialView("ProductPartial", dsSanPham);
        }
    }
}