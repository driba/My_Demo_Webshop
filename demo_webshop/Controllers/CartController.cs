using demo_webshop.Data;
using demo_webshop.Extensions;
using demo_webshop.Models;
using Microsoft.AspNetCore.Mvc;

namespace demo_webshop.Controllers
{
    public class CartController : Controller
    {
        // Kljuc nase sesije za kosaricu
        public const string SessionKeyName = "_cart";
        // Objekt za pristup bazi podataka
        public readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Cart/Index
        public IActionResult Index()
        {
            List<CartItem> cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>(SessionKeyName) ?? new List<CartItem>();

            return View(cart);
        }

        // TODO: AddToCart(int productId, decimal quantity)
        // POST: Cart/AddToCart
        [HttpPost]
        public IActionResult AddToCart(int productId, decimal quantity)
        {
            // Korak 1: Provjeri sesiju i ako postoji proizvod
            List<CartItem> cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>(SessionKeyName) ?? new List<CartItem>();

            // 1.1.
            Product find_product = _context.Products.Find(productId);
            if (find_product == null)
            {
                return RedirectToAction("Index");
            }           

            if (cart.Count == 0)
            {
                // Korak 2: Ako je kosarica prazna ili null, kreiraj objekt klase CartItem i
                // popuni ga podacima pa spremi sesiju
                if (quantity > find_product.Quantity)
                {
                    return RedirectToAction("Product", "Home", new { msg = "Nije moguće dodati proizvod u košaricu." }); // bolje vratiti u Product/Index
                }

                CartItem new_item = new CartItem()
                {
                    Product = find_product,
                    Quantity = quantity
                };
                cart.Add(new_item);

                HttpContext.Session.SetObjectAsJson(SessionKeyName, cart);
                
            }
            else
            {
                // Korak 3: Ako kosarica nije prazna, dodaj novi proizvod ili azuriraj kolicinu postojeceg,
                // pa pohrani opet u sesiju
            }

            return RedirectToAction("Index");
        }
        // TODO: RemoveFromCart(int productId)

    }
}
