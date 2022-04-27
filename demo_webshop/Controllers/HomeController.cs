using demo_webshop.Data;
using demo_webshop.Extensions;
using demo_webshop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace demo_webshop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context; // Dependency injection
        private const string SessionKeyName = "_cart";
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            // Jednostavni test za provjeru ako je sesija aktivna
            // ViewBag.CheckSession = HttpContext.Session.IsAvailable;

            // Test za kreiranje sesije
            HttpContext.Session.SetString("test", "Hej, ja sam sesija");
            // 
            ViewBag.GetTest = HttpContext.Session.GetString("test");

            return View();
        }

        public IActionResult Privacy()
        {
            ViewBag.GetTest = HttpContext.Session.GetString("test");

            return View();
        }

        // GET; /home/Products
        public IActionResult Product(int? categoryId, string? message)
        {

            //List<Product> products = _context.Products.ToList();
            List<Product> products = (categoryId != null) ?
                // ako categoryId postoji
            _context.Products.Where( // popis svih proizvoda i postavljamo kriterij
                p => _context.ProductCategories.Where(
                    pc => pc.CategoryId == categoryId  // ako je u tablici ProductCategories vrijednost stupca CategoryId = categoryId
                    ).Select(
                        pc => pc.ProductId // ako je kriterij zadovoljen, vrati vrijednost stupca productId
                        ).ToList().Contains(
                            p.Id  // Nakon toga, vrati objekte klase Product, čiji ID se nalazi u rezultatu kriterija
                        )
                        ).ToList() :
                        //ako categoryId ne postoji
            _context.Products.ToList();

            if (message != null)
            {
                ViewBag.Message = message;
            }
            
            

            ViewBag.Categories = _context.Categories.ToList();

            return View(products);
        }

        // GET: Home/Order
        public IActionResult Order()
        {
            List<CartItem> cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>(SessionKeyName) ?? new List<CartItem>();

            if (cart.Count == 0)
            {
                return RedirectToAction("Product");
            }

            decimal sum = 0;
            ViewBag.TotalPrice = cart.Sum(item => sum + item.GetTotal());

            return View(cart);
        
        }

        // POST: Home/Order


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}