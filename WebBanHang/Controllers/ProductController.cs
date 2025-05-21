using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebBanHang.Models;

namespace WebBanHang.Controllers
{
    public class ProductController : Controller
    {
        private ApplicationDbContext _db;
        private IWebHostEnvironment _hosting;
        public ProductController(ApplicationDbContext db, IWebHostEnvironment hosting)
        {
            _db = db;
            _hosting = hosting;
        }

        #region Xem_DSSP
        public IActionResult Index(int page=1)
        {
            var pagesize = 5;
            var currentpage = page;
            //Đọc tất cả sản phẩm trong CSDLthông qua dbcontext
            //var dsSanPham = _db.Products.Where(x=>x.Price>400).OrderBy(p=>p.Price).ToList();//Linq (method syntax)
            //var dsSanPham = (from x in _db.Products where x.Price>400 select x).ToList();//Linq ( query syntax)
            var dsSanPham = _db.Products.Include(x => x.Category).ToList();
            ViewBag.PageSum = Math.Ceiling((double)dsSanPham.Count / pagesize);
            ViewBag.CurrentPage = currentpage;
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("ProductPatrial",dsSanPham.Skip((currentpage - 1) * pagesize).Take(pagesize).ToList());
            }
            return View(dsSanPham.Skip((currentpage - 1) * pagesize).Take(pagesize).ToList());
            //return View(dsSanPham);
        }
        #endregion
        //Xử lý xóa sản phẩm
        #region XuLy_Xoa
        public IActionResult Delete(int id) {
            //1.Truy vấn sản phẩm cần xóa trong CSDL
            var sp = _db.Products.Find(id); //=> Truy vấn theo khóa chính
            //4.Điều hướng người dùng về lại action index
            return View(sp);

        }
        //Thông báo xác nhận 
        public IActionResult DeleteConfirm(int id)
        {
            //1.Truy vấn sản phẩm cần xóa trong CSDL
            var sp = _db.Products.Find(id); //=> Truy vấn theo khóa chính
            //Cách 2
            //var sp=_db.Products.Where(x=>x.Id==id).FirstOrDefault();
            if (!String.IsNullOrEmpty(sp.ImageUrl))
            {
                var oldFilePath = Path.Combine(_hosting.WebRootPath, sp.ImageUrl);
                if (System.IO.File.Exists(oldFilePath))
                {
                    System.IO.File.Delete(oldFilePath);
                }
            }
            //2.Thực hiện xóa sản phẩm
            _db.Products.Remove(sp);
            _db.SaveChanges();
            TempData["success"] = "Xóa sản phẩm thành công";
            //3.Thông báo kết quả thao tác
            //4.Điều hướng người dùng về lại action index
            return RedirectToAction("Index");

        }
        #endregion
        #region XuLy_ThemSp
        [HttpPost]
        public IActionResult Add(Product product, IFormFile imageUrl) {
            //if (ModelState.IsValid) {
            if (imageUrl != null)
            {
                product.ImageUrl = SaveImage(imageUrl);
            }
            //Thêm product vào table product
            _db.Products.Add(product);
            _db.SaveChanges();
            TempData["success"] = "Thêm sản phẩm thành công";
            //    return RedirectToAction("Index");
            //}
            //ViewBag.CategoryList = _db.Categoríe.Select(x => new SelectListItem
            //{
            //    Value = x.Id.ToString(),
            //    Text = x.Name
            //});
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.DSTL = _db.Categoríe.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            });
            return View();
        }
        #endregion
        private String SaveImage(IFormFile imageUrl)
        {
            //Đặt lại tên file cần lưu
            var filename = Guid.NewGuid().ToString() + Path.GetExtension(imageUrl.FileName);
            //Lấy đường dẫn lưu trữ trên root server
            var path = Path.Combine(_hosting.WebRootPath, @"image/products");
            var save = Path.Combine(path, filename);
            using (var filestream = new FileStream(save, FileMode.Create))
            {
                imageUrl.CopyTo(filestream);
            }
            return @"image/products/" + filename;
        }
        #region XuLy_SuaSP
        [HttpPost]
        public IActionResult Update(Product product, IFormFile ImageUrl)
        {
            //B1: Truy vấn sản phẩm cần cập nhật trong CSDL
            var OldProduct=_db.Products.Find(product.Id);
            if (ImageUrl != null)
            {
                //xử lý upload và lưu ảnh đại diện mới
                product.ImageUrl = SaveImage(ImageUrl);
                //xóa ảnh cũ (nếu có)
                if (!string.IsNullOrEmpty(product.ImageUrl))
                {
                    var oldFilePath = Path.Combine(_hosting.WebRootPath, product.ImageUrl);
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }
            }
            else
            {
                product.ImageUrl = OldProduct.ImageUrl;
               // product.ImageUrl=SaveImage(ImageUrl);
            }
            //cập nhật product vào table Product
            OldProduct.Name = product.Name;
            OldProduct.Description = product.Description;
            OldProduct.Price = product.Price;
            OldProduct.CategoryId = product.CategoryId;
            OldProduct.ImageUrl = product.ImageUrl;
            _db.SaveChanges();
            TempData["success"] = "Cập nhật sản phẩm thành công";
            return RedirectToAction("Index");

        }
       


        //Hiển thị form cập nhật sản phẩm
        public IActionResult Update(int id)
        {
            //Truy vấn sản phẩm cần cập nhật trong CSDL
            //Cách 1: Dùng biểu thức có điều kiện
            //var sp = _db.Products.Where(x=>x.Id==id).SingleOrDefault();
            //Cách 2: Tìm theo khóa chính
            var sp=_db.Products.Find(id);            
            //truyền danh sách thể loại cho View để sinh ra điều khiển DropDownList
            ViewBag.CategoryList = _db.Categoríe.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            });
            return View(sp);
        }
        #endregion
    }
}
