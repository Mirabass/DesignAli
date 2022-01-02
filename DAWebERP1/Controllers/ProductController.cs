using DAWebERP1.Models.Product;
using DAWebERP1.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using DAWebERP1.BusinessLogic;
using DAWebERP1.Services;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DAWebERP1.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IColorProvider _colorProvider;

        public ProductController(ApplicationDbContext db, IColorProvider colorProvider)
        {
            _db = db;
            _colorProvider = colorProvider;
        }

        public IActionResult Index()
        {
            IEnumerable<ProductModel> products = _db.Products
                .Include(product => product.ProductStrap)
                .Include(product => product.ProductColorDesign)
                .Include(product => product.ProductDivision)
                    .ThenInclude(pd => pd.ProductKind)
                .Include(product => product.ProductDivision)
                    .ThenInclude(pd => pd.ProductMaterial);
            foreach (ProductModel product in products)
            {
                product.ProductColorDesign.MainPartColorHex = _colorProvider.GetHexFromRal(product.ProductColorDesign.MainPartRAL);
            }
            return View(products);
        }
        // GET-Create
        public IActionResult Create()
        {
            // TODO: ProductNames to view bag
            var list = _db.ProductDivisions.Select(pd =>
                                                        new SelectListItem
                                                        {
                                                            Value = pd.Id.ToString(),
                                                            Text = pd.Name
                                                        });
            ViewBag.ProductNames = new SelectList(list, "Value", "Text");

            return View();
        }
        // POST-Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProductModel product)
        {
            ProductDivisionModel selectedProductDivision = _db.ProductDivisions
                .Include(pd => pd.ProductKind)
                .Include(pd => pd.ProductMaterial)
                .Where(pd => pd.Id == product.ProductDivision.Id)
                .FirstOrDefault();
            product.ProductDivision = selectedProductDivision;
            CustomOperations.CreateAndAsignDesignationFor(product);
            _db.Add(product);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
