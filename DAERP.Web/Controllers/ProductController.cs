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
using Microsoft.AspNetCore.Mvc.ModelBinding;

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
                Helper.StaticHelper.SetDataForSortingPurposes(ViewData, sortOrder, products.FirstOrDefault(), defaultPropToSort);
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
        [HttpPost]
        public JsonResult RetrieveProductImage(int? Id)
        {
            string notFoundResult = @"https://www.w3schools.com/images/lamp.jpg";
            if (Id == null || Id == 0)
            {
                return Json(notFoundResult);
            }
            ProductImageModel productImage = _productData.GetProductImageBy(Id);
            if (productImage == null)
            {
                return Json(notFoundResult);
            }
            string productImageDataURL = Helper.StaticHelper.ConvertImageToURL(productImage.Image, productImage.Type);
            return Json(productImageDataURL);
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
        public async Task<IActionResult> Create(ProductCreateViewModel productViewModel)
        {
            if (ModelState.IsValid)
            {
                ProductDivisionModel selectedProductDivision = _productData.GetProductDivisionBy(productViewModel.ProductDivisionId);
                ProductModel product = _mapper.Map<ProductModel>(productViewModel);
                product.ProductDivision = selectedProductDivision;
                CustomOperations.CreateAndAsignDesignationFor(product);
                product.DateCreated = System.DateTime.Today;
                product.DateLastModified = System.DateTime.Today;
                try
                {
                    if (Request.Form.Files.Count != 0)
                    {
                        product.ProductImage = ImageFromFile(Request.Form.Files.FirstOrDefault(), ModelState);
                    }
                }
                catch (Exception)
                {
                    CreateViewBagOfProductNames();
                    return View(productViewModel);
                }
                product.ProductPrices.GainPercentValue = BL.PriceCalculation.GainPercentValue(product.ProductPrices.OperatedCostPrice, product.ProductPrices.OperatedSellingPrice);
                await _productData.AddProductAsync(product);
                await _productData.UpdateProductCustomersPricesAsync(product);
                return RedirectToAction("Index");
            }
            CreateViewBagOfProductNames();
            return View(productViewModel);
        }

        private ProductImageModel ImageFromFile(Microsoft.AspNetCore.Http.IFormFile file, ModelStateDictionary modelState)
        {
            ProductImageModel img = new ProductImageModel();
            using MemoryStream ms = new MemoryStream();
            file.CopyTo(ms);
            if (ms.Length > 2097152) // 2 MB
            {
                ModelState.AddModelError("Image", "The Image is too large");
                throw new FileLoadException();
            }
            img.Image = ms.ToArray();
            string fileType = Path.GetExtension(file.FileName).Replace(".", String.Empty).ToUpper();
            if ((fileType == "PNG" || fileType == "JPG") == false)
            {
                ModelState.AddModelError("Invalid image file name", "Invalid file name of image. Must be PNG or JPG.");
                throw new FormatException();
            }
            img.Type = fileType;
            return img;
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
            List<CustomerProductModel> customersWithStock = _customerProductData.GetCustomersWithStockOfProductWithChildModelsIncludedBy(Id).ToList();
            if (customersWithStock.Count > 0)
            {
                string customersWithStockMessage = "";
                customersWithStock.ForEach(p =>
                {
                    customersWithStockMessage += p.Customer.Designation + ": " + p.AmountInStock + "\n";
                });
                return Content("Není možné smazat tento výrobek, protože ho mají naskladněny následující odběratelé:\n" + customersWithStockMessage);
            }
            ProductCreateViewModel productViewModel = _mapper.Map<ProductCreateViewModel>(product);
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
            ProductCreateViewModel productViewModel = _mapper.Map<ProductCreateViewModel>(product);
            CreateViewBagOfProductNames();
            return View(productViewModel);
        }
        // POST-Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Update(ProductCreateViewModel productViewModel)
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
                try
                {
                    if (Request.Form.Files.Count != 0)
                    {
                        updatedProduct.ProductImage = ImageFromFile(Request.Form.Files.FirstOrDefault(), ModelState);
                    }
                }
                catch (Exception)
                {
                    CreateViewBagOfProductNames();
                    return View(productViewModel);
                }
                updatedProduct.ProductPrices.GainPercentValue =
                    BL.PriceCalculation.GainPercentValue(updatedProduct.ProductPrices.OperatedCostPrice,
                    updatedProduct.ProductPrices.OperatedSellingPrice);
                updatedProduct.MainStockValue = updatedProduct.MainStockAmount * updatedProduct.ProductPrices.OperatedCostPrice;
                _productData.UpdateProduct(updatedProduct);
                await _productData.UpdateProductCustomersPricesAsync(updatedProduct);
                return RedirectToAction("Index");
            }
            CreateViewBagOfProductNames();
            return View(productViewModel);
        }
    }
}
