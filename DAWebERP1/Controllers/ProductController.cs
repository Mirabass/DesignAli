using DAWebERP1.Models.Product;
using DAWebERP1.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using DAWebERP1.Models;
using System.IO;
using Newtonsoft.Json;
using DAWebERP1.Services;

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
        public IActionResult Create()
        {
            // TODO: ProductNames to view bag
            return View();
        }
    }
}
