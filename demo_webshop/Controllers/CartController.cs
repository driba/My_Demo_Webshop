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

            decimal sum = 0;
            ViewBag.TotalPrice = cart.Sum(item => sum + item.GetTotal());

            return View(cart);
        }

        // TODO: AddToCart(int productId, decimal quantity)
        // POST: Cart/AddToCart
        [HttpPost]
        public IActionResult AddToCart(int productId, decimal quantity)
        {
            //TODO: Ispraviti BUG koji omogućuje dodavanje proizvoda s 0 količinom!
            if (quantity <= 0)
            {
                return RedirectToAction("Product", "Home", new { message = "Molimo odaberite količinu proizvoda." }); 
            }

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
                    return RedirectToAction("Product", "Home", new { message = "Nije moguće dodati proizvod u košaricu." }); // bolje vratiti u Product/Index
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
                // Korak 3: Ako kosarica nije prazna, a isti proizvod postoji, azuriraj kolicinu postojeceg,
                // pa pohrani opet u sesiju

                // Provjeravamo stavke kosarice, ali proizvod ne postoji u njoj, dodaj ga i pohrani opet u sesiju
                var update_or_create_product = cart.Find(p => p.Product.Id == productId) ?? new CartItem();

                if (quantity + update_or_create_product.Quantity > find_product.Quantity)
                {
                    return RedirectToAction("Product", "Home", new { message = "Nije dozvoljena dodana količina, maksimalno je dozvoljeno " + find_product.Quantity });
                }

                if (update_or_create_product.Quantity == 0)
                {
                    update_or_create_product.Product = find_product;
                    update_or_create_product.Quantity = quantity;
                    cart.Add(update_or_create_product);
                }
                else
                {
                    update_or_create_product.Quantity += quantity;
                }

                HttpContext.Session.SetObjectAsJson(SessionKeyName, cart);
            }

            return RedirectToAction("Index");
        }


        // TODO: RemoveFromCart(int productId)
        public IActionResult RemoveFromCart(int productId)
        {
            List<CartItem> cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>(SessionKeyName) ?? new List<CartItem>();

            cart.RemoveAll(p => p.Product.Id == productId);

            HttpContext.Session.SetObjectAsJson(SessionKeyName, cart);

            return RedirectToAction("Index");
        }
    }
}
