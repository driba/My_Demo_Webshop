﻿using demo_webshop.Data;
using demo_webshop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace demo_webshop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ProductController : Controller
    {

        private readonly ApplicationDbContext _context;
        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }


        // GET: Admin/Product
        public ActionResult Index()
        {

            return View(_context.Products.ToList());
        }

        // GET: ProductController/Details/5
        public ActionResult Details(int id)
        {
            // Provjera
            if (id == 0)
            {
                return RedirectToAction("Index");
            }

            // Dohvat proizvoda
            var product = _context.Products.SingleOrDefault(p=> p.Id == id);

            // Provjera ako proizvod postoji
            if (product == null)
            {
                return RedirectToAction("Index", new { msg = "Proizvod ne postoji"});
            }

            // Vrati nam listu kategorija s kojom je proizvod povezan
            //List<Category> categories = _context.ProductCategories.Where(s => s.ProductId == id)
            //    .Select(
            //        s => new Category
            //        {
            //            Id = s.CategoryId,
            //            Title = _context.Categories.Where(p => p.Id == s.CategoryId).First().Title
            //        }                    
            //    ).ToList();

            var prod_cat = _context.ProductCategories.Where(pc => pc.ProductId == id).Select(pc => pc.CategoryId).ToList();
            var categories = _context.Categories.Where(c => prod_cat.Contains(c.Id)).Select(c => c.Title).ToList();
            ViewBag.Categories = categories;
            return View(product);
        }

        // GET: Admin/Product/Create
        public ActionResult Create(string? error_message)
        {

            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.CategoryErrorMessage = error_message;

            return View();
        }

        // POST: Admin/Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product, int[] category_id)
        {
            // 1. korak -> provjeri ako category_id nije prazan ili null
            if(category_id.Length == 0)
            {
                // Create("Molimo odaberite minimalno jednu kategoriju");
                return RedirectToAction("Create", new { error_message = "Molimo odaberite minimalno jednu kategoriju" });
            }


            try
            {
                // 2. korak -> unesi proizvod i vrati "Id" unesenog proizvoda
                _context.Products.Add(product);
                _context.SaveChanges(); // nakon pohrane u tablicu, dobiva se vrijednost Id

                int idProduct = product.Id;


                // 3. korak -> iteriraj kroz vrijednosti varijable "category_id" i pohrani sve u tablicu ProductCategory

                foreach (var category in category_id)
                {
                    ProductCategory ProductCategory = new ProductCategory();
                    ProductCategory.ProductId = idProduct;
                    ProductCategory.CategoryId = category;

                    _context.ProductCategories.Add(ProductCategory);
                    
                }
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                return RedirectToAction("Create", new { error_message = ex.Message });
            }
        }

        // GET: Admin/Product/Edit/5
        public ActionResult Edit(int id, string? error_message)
        {
            // Provjera
            if (id == 0)
            {
                return RedirectToAction("Index");
            }

            // Dohvat proizvoda
            var product = _context.Products.SingleOrDefault(p => p.Id == id);

            // Provjera ako proizvod postoji
            if (product == null)
            {
                return RedirectToAction("Index", new { msg = "Proizvod ne postoji" });
            }

            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.CategoryErrorMessage = error_message;
            ViewBag.CheckedCategories = _context.ProductCategories.Where(pd => pd.ProductId == id).Select(pd => pd.CategoryId).ToList();

            return View(product);
        }

        // POST: Admin/Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product, int[] category_id)
        {
            if (category_id.Length == 0)
            {
                return Edit(product.Id, "Molimo odaberite minimalno jednu kategoriju");
            }

            try
            {
                _context.Products.Update(product);

                // "stare" kategorije datog proizvoda
                var prod_cat_list = _context.ProductCategories.Where(pd => pd.ProductId == product.Id).ToList();
                
                // dodavanje "eventualno" novih kategorija
                foreach (var category in category_id)
                {
                    if (!prod_cat_list.Select(pd => pd.CategoryId).Contains(category))
                    {
                        ProductCategory ProductCategory = new ProductCategory();
                        ProductCategory.ProductId = product.Id;
                        ProductCategory.CategoryId = category;

                        _context.ProductCategories.Add(ProductCategory);
                    }
                }

                // brisanje "eventualnog" viska kategorija

                foreach (var prod_cat in prod_cat_list)
                {
                    if (!category_id.Contains(prod_cat.CategoryId))
                    {
                        _context.ProductCategories.Remove(prod_cat);
                    }
                }


                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Product/Delete/5
        public ActionResult Delete(int id)
        {
            // Provjera
            if (id == 0)
            {
                return RedirectToAction("Index");
            }

            // Dohvat proizvoda
            var product = _context.Products.SingleOrDefault(p => p.Id == id);

            // Provjera ako proizvod postoji
            if (product == null)
            {
                return RedirectToAction("Index", new { msg = "Proizvod ne postoji" });
            }
            return View(product);
        }

        // POST: Admin/Product/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            // Provjera
            if (id == 0)
            {
                return RedirectToAction("Index");
            }

            try
            {
                // Pronađi proizvod
                var find_product = _context.Products.SingleOrDefault(s => s.Id == id);

                if (find_product == null)
                {
                    return View("Delete", new { msg = "Proizvod ne postoji" });
                }

                // Test 1: Obrisi samo proizvod
                // EF -> podesio vanjski ključ onDelete: Cascade
                _context.Products.Remove(find_product);
                _context.SaveChanges();

                // Procedura sa SQL upitima (ako test 1 ne radi ili ako nije OnDelete: Cascade)
                // 1. korak -> brisanje svh zapisa gdje je ID proizvoda vanjski ključ
                // 2. korak -> brisanje zapisa proizvoda

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}