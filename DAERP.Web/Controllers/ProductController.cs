using DAERP.BL.Models.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using DAERP.BL;
using DAERP.DAL.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using DAERP.Web.ViewModels;
using DAERP.DAL.DataAccess;
using AutoMapper;
using DAERP.BL.Models;
using System.Threading.Tasks;
using System;
using DAERP.Web.Helper;
using System.IO;

namespace DAERP.Web.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IProductData _productData;
        private readonly ICustomerProductData _customerProductData;
        private readonly IColorProvider _colorProvider;
        private readonly IMapper _mapper;

        public ProductController(IColorProvider colorProvider, IProductData productData, IMapper mapper, ICustomerProductData customerProductData)
        {
            _colorProvider = colorProvider;
            _productData = productData;
            _mapper = mapper;
            _customerProductData = customerProductData;
        }
        [Authorize(Roles = "Admin,Manager,Cashier")]
        public IActionResult Index(
            string currentSort,
            string sortOrder,
            string currentFilter,
            string searchString,
            int? pageNumber)
        {
            if (sortOrder is null)
            {
                sortOrder = currentSort;
            }
            ViewData["CurrentSort"] = sortOrder;
            ViewData["CurrentFilter"] = searchString ?? currentFilter;
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            IEnumerable<ProductModel> products = _productData.GetAllProductsWithChildModelsIncluded();
            if (!String.IsNullOrEmpty(searchString))
            {
                string normalizedSearchString = searchString.Normalize(System.Text.NormalizationForm.FormD).ToUpper();
                products = products.Where(p => 
                    p.Designation.Normalize(System.Text.NormalizationForm.FormD).ToUpper().Contains(normalizedSearchString) ||
                    p.EAN.ToString().Contains(normalizedSearchString) ||
                    p.ProductDivision.Name.Normalize(System.Text.NormalizationForm.FormD).ToUpper().Contains(normalizedSearchString) ||
                    p.ProductDivision.ProductType.Normalize(System.Text.NormalizationForm.FormD).ToUpper().Contains(normalizedSearchString)
                );
            }
            if (products.Count() > 0)
            {
                string defaultPropToSort = "Designation";
                Helper.Helper.SetDataForSortingPurposes(ViewData, sortOrder, products.FirstOrDefault(), defaultPropToSort);
                foreach (ProductModel product in products)
                {
                    
                    product.ProductColorDesign.MainPartColorHex = _colorProvider.GetHexFromRal(product.ProductColorDesign.MainPartRAL);
                    product.ProductColorDesign.PocketColorHex = _colorProvider.GetHexFromRal(product.ProductColorDesign.PocketRAL);
                    product.ProductStrap.ColorHex = _colorProvider.GetHexFromRal(product.ProductStrap.RAL);
                }
                if (String.IsNullOrEmpty(sortOrder))
                {
                    sortOrder = defaultPropToSort;
                }
                bool descending = false;
                if (sortOrder.EndsWith("_desc"))
                {
                    sortOrder = sortOrder.Substring(0, sortOrder.Length - 5);
                    descending = true;
                }
                if (descending)
                {
                    products = products.OrderByDescending(e => DataOperations.GetPropertyValue(e, sortOrder));
                }
                else
                {
                    products = products.OrderBy(e => DataOperations.GetPropertyValue(e, sortOrder));
                }
            }
            int pageSize = 12;
            return View(PaginatedList<ProductModel>.Create(products, pageNumber ?? 1, pageSize));
        }

        // GET-Create
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Create()
        {
            CreateViewBagOfProductNames();
            return View();
        }

        private void CreateViewBagOfProductNames()
        {
            var productDivisions = _productData.GetAllProductDivisions();
            var list = productDivisions.Select(pd =>
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
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Create(ProductViewModel productViewModel)
        {
            if (ModelState.IsValid)
            {
                ProductDivisionModel selectedProductDivision = _productData.GetProductDivisionBy(productViewModel.ProductDivisionId);
                ProductModel product = _mapper.Map<ProductModel>(productViewModel);
                product.ProductDivision = selectedProductDivision;
                CustomOperations.CreateAndAsignDesignationFor(product);
                product.DateCreated = System.DateTime.Today;
                product.DateLastModified = System.DateTime.Today;
                ProductImageModel img = new ProductImageModel();
                var file = Request.Form.Files.FirstOrDefault();
                MemoryStream ms = new MemoryStream();
                file.CopyTo(ms);
                img.Image = ms.ToArray();
                ms.Close();
                ms.Dispose();
                product.ProductImage = img;
                await _productData.AddProduct(product);
                return RedirectToAction("Index");
            }
            CreateViewBagOfProductNames();
            return View(productViewModel);
        }
        // Get-Delete
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Delete(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            ProductModel product = _productData.GetProductWithChildModelsIncludedBy(Id);
            if (product == null)
            {
                return NotFound();
            }
            List<CustomerProductModel> customersWithStock = _customerProductData.GetCustomersWithStockOfProductBy(Id).ToList();
            if (customersWithStock.Count > 0)
            {
                string customersWithStockMessage = "";
                customersWithStock.ForEach(p =>
                {
                    customersWithStockMessage += p.Customer.Designation + ": " + p.AmountInStock + "\n";
                });
                return Content("Není možné smazat tento výrobek, protože ho mají naskladněny následující odběratelé:\n" + customersWithStockMessage);
            }
            ProductViewModel productViewModel = _mapper.Map<ProductViewModel>(product);
            ViewBag.ProductName = _productData.GetProductDivisionNameBy(product.ProductDivision.Id);
            return View(productViewModel);
        }
        // POST-Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult DeletePost(int? Id)
        {
            ProductModel product = _productData.GetProductWithChildModelsIncludedBy(Id);
            if (product == null)
            {
                return NotFound();
            }
            else
            {
                _productData.RemoveProduct(product);
                return RedirectToAction("Index");
            }
        }

        // GET-Update
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Update(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            ProductModel product = _productData.GetProductWithChildModelsIncludedBy(Id);
            if (product == null)
            {
                return NotFound();
            }
            ProductViewModel productViewModel = _mapper.Map<ProductViewModel>(product);
            CreateViewBagOfProductNames();
            return View(productViewModel);
        }
        // POST-Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Update(ProductViewModel productViewModel)
        {
            if (ModelState.IsValid)
            {
                ProductModel oldProduct = _productData.GetProductWithChildModelsIncludedBy(productViewModel.Id);
                ProductDivisionModel updatedProductDivision = _productData.GetProductDivisionWithChildModelsIncludedBy(productViewModel.ProductDivisionId);
                ProductModel updatedProduct = _mapper.Map<ProductModel>(productViewModel);
                updatedProduct.ProductDivision = updatedProductDivision;
                updatedProduct.DateCreated = oldProduct.DateCreated;
                CustomOperations.CreateAndAsignDesignationFor(updatedProduct);
                updatedProduct.DateLastModified = System.DateTime.Today;
                _productData.UpdateProduct(updatedProduct);
                return RedirectToAction("Index");
            }
            CreateViewBagOfProductNames();
            return View(productViewModel);
        }
    }
}
