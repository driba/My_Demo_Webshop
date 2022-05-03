using demo_webshop.Data;
using demo_webshop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace demo_webshop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        

        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Order
        public IActionResult Index()
        {
            return View(_context.Orders);
        }

        // GET: Admin/Details/5
        public IActionResult Details(int id)
        {
            // 1. korak - provjera ako varijabla id nije null
            if (id == 0)
            {
                return RedirectToAction("Index");
            }

            // 2. korak - dohvat narudzbe iz baze prema ID-u
            var order = _context.Orders.FirstOrDefault(o => o.Id == id);

            // 3. korak - provjera ako narudzba postoji
            if (order == null)
            {
                return RedirectToAction("Index", new { msg = "Narudžba ne postoji" });
            }

            // 4. korak - dohvat stavki narudzbe
            List<OrderItem> order_items = (
                from order_item in _context.OrderItems
                where order_item.OrderId == id
                select new OrderItem
                {
                    Id = order_item.Id,
                    OrderId = order_item.OrderId,
                    ProductId = order_item.ProductId,
                    Quantity = order_item.Quantity,
                    Price = order_item.Price,
                    Total = order_item.Total,
                    ProductTitle = _context.Products.FirstOrDefault(p => p.Id == order_item.ProductId).Title
                }
                ).ToList();
            ViewBag.OrderItems = order_items;

            return View(order);
        }

        // GET: Admin/Order/Create
        public IActionResult Create()
        {
            return View();        
        }

        //POST: Admin/Order/Create
        [HttpPost]
        public IActionResult Create(Order new_order)
        {
            new_order.DateCreated = DateTime.Now;
            new_order.Total = 0;

            if (ModelState.IsValid)
            {
                _context.Orders.Add(new_order);
                _context.SaveChanges();

                // ovdje smo nakon pohrane dobili ID narudzbe
                return RedirectToAction("AddOrderItems", new { id = new_order.Id });
            }

            return View();
        }


        // GET: Admin/Order/AddOrderItems/5
        public IActionResult AddOrderItems(int id)
        {
            ViewBag.OrderId = id;
            ViewBag.Products = _context.Products.ToList();

            return View();
        }

        // POST: Admin/Order/AddOrderItems/5
        [HttpPost]
        public IActionResult AddOrderItems(OrderItem new_order_item)
        {
            // 1. korak - pronadi proizvod za detalje
            Product find_product = _context.Products.FirstOrDefault(p => p.Id == new_order_item.ProductId);

            // 2. korak - provjeri ako je kolicina zadovoljacajuca
            if (new_order_item.Quantity > find_product.Quantity)
            {
                return RedirectToAction("AddOrderItems", new { id = new_order_item.OrderId});
            }

            // 3. korak - azuriraj ostala svojstva parametra stavke narudzbe
            new_order_item.Price = find_product.Price;
            new_order_item.Total = new_order_item.Quantity * new_order_item.Price;
            _context.OrderItems.Add(new_order_item);
            _context.SaveChanges();

            // 4. korak - azuriraj ukupnu cijenu narudzbe
            Order find_order = _context.Orders.FirstOrDefault(o => o.Id == new_order_item.OrderId);
            find_order.Total += new_order_item.Total;

            _context.Orders.Update(find_order);
            _context.SaveChanges();

            // 5. korak - preusmjeri i nastavi s unosom
            return RedirectToAction("AddOrderItems", new { id = new_order_item.OrderId, msg = "Stavka uspješno dodana!" });



        }
    }
}
