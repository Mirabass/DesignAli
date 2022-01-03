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
                var colorHex = _colorProvider.GetHexFromRal(product.ProductColorDesign.MainPartRAL);
                product.ProductColorDesign.MainPartColorHex = colorHex != null ? colorHex : "#FFFFFF";
            }
            return View(products);
        }
        // GET-Create
        public IActionResult Create()
        {
            CreateViewBagOfProductNames();
            return View();
        }

        private void CreateViewBagOfProductNames()
        {
            var list = _db.ProductDivisions.Select(pd =>
                                            new SelectListItem
                                            {
                                                Value = pd.Id.ToString(),
                                                Text = pd.Name
                                            });
            ViewBag.ProductNames = new SelectList(list, "Value", "Text");
        }

        // POST-Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProductModel product)
        {
            if (ModelState.IsValid)
            {
                ProductDivisionModel selectedProductDivision = _db.ProductDivisions
                        .Include(pd => pd.ProductKind)
                        .Include(pd => pd.ProductMaterial)
                        .Where(pd => pd.Id == product.ProductDivision.Id)
                        .FirstOrDefault();
                product.ProductDivision = selectedProductDivision;
                CustomOperations.CreateAndAsignDesignationFor(product);
                ProductColorDesignModel productColorDesign = new ProductColorDesignModel();
                ProductStrapModel productStrap = new ProductStrapModel();
                product.ProductColorDesign = productColorDesign;
                product.ProductStrap = productStrap;
                product.DateCreated = System.DateTime.Today;
                product.DateLastModified = System.DateTime.Today;
                _db.Add(productColorDesign);
                _db.Add(productStrap);
                _db.Add(product);
                _db.SaveChanges();
                return RedirectToAction("Index"); 
            }
            CreateViewBagOfProductNames();
            return View(product);
        }

    }
}
