using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBanHang.Models;

namespace WebBanHang.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;
        public HomeController(ApplicationDbContext db,ILogger<HomeController> logger)
        {
            _db = db;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var pageSize = 3;
                var dsSanPham = _db.Products.Include(x => x.Category ).ToList();
            return View(dsSanPham.Skip(0).Take(pageSize).ToList());
        }


        public IActionResult LoadMore(int page = 1)
        {
            var pageSize = 3;
            var products = _db.Products.ToList();
            return PartialView("_ProductPartial", products.Skip((page - 1) * pageSize).Take(pageSize).ToList());
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
