using DAERP.BL.Models;
using DAERP.BL.Models.Product;
using DAERP.DAL.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAERP.DAL.DataAccess
{
    public class ProductData : IProductData
    {
        private ApplicationDbContext _db;
        public ProductData(ApplicationDbContext db)
        {
            _db = db;
        }
        public IEnumerable<ProductModel> GetAllProductsWithChildModelsIncluded()
        {
            IEnumerable<ProductModel> products = _db.Products
                .Include(product => product.ProductStrap).AsNoTracking()
                .Include(product => product.ProductColorDesign).AsNoTracking()
                .Include(product => product.ProductPrices).AsNoTracking()
                .Include(product => product.ProductDivision)
                    .ThenInclude(pd => pd.ProductKind).AsNoTracking()
                .Include(product => product.ProductDivision)
                    .ThenInclude(pd => pd.ProductMaterial).AsNoTracking()
                .Include(product => product.ProductCustomers).AsNoTracking();    
            return products;
        }
        public IEnumerable<ProductDivisionModel> GetAllProductDivisions()
        {
            return _db.ProductDivisions.AsNoTracking();
        }

        public ProductDivisionModel GetProductDivisionBy(int productDivisionId)
        {
            ProductDivisionModel productDivision = _db.ProductDivisions
                .Include(pd => pd.ProductKind).AsNoTracking()
                .Include(pd => pd.ProductMaterial).AsNoTracking()
                .Where(pd => pd.Id == productDivisionId)
                .FirstOrDefault();
            return productDivision;
        }

        public async Task AddProductAsync(ProductModel product)
        {
            product.ProductCustomers = new List<CustomerProductModel>();
            await _db.Customers.ForEachAsync(c =>
            {
                product.ProductCustomers.Add(new CustomerProductModel()
                {
                    ProductId = product.Id,
                    CustomerId = c.Id
                });
            });

            await _db.AddAsync(product);
            _db.Entry(product.ProductDivision).State = EntityState.Unchanged;
            _db.Entry(product.ProductDivision.ProductKind).State = EntityState.Unchanged;
            _db.Entry(product.ProductDivision.ProductMaterial).State = EntityState.Unchanged;

            await _db.SaveChangesAsync();
        }
        public async Task UpdateProductCustomersPricesAsync(ProductModel product)
        {
            await _db.CustomersProducts
                .Include(cp => cp.Product)
                    .ThenInclude(cp => cp.ProductPrices)
                .Include(cp => cp.Customer)
                .Where(cp => cp.ProductId == product.Id)
                .ForEachAsync(pc =>
                {
                    pc.DeliveryNotePrice = BL.PriceCalculation.DeliveryNotePrice(
                        pc.Product.ProductPrices.GainPercentValue,
                        pc.Customer.ProvisionFor60PercentValue,
                        pc.Product.ProductPrices.OperatedCostPrice);
                    pc.IssuedInvoicePrice = BL.PriceCalculation.IssuedInvoicePrice(
                        pc.DeliveryNotePrice,
                        pc.Customer.FVDiscountPercentValue);
                    pc.Value = BL.PriceCalculation.StockValue(
                        pc.AmountInStock,
                        pc.DeliveryNotePrice);
                });
            await _db.SaveChangesAsync();
        }

        public ProductModel GetProductWithChildModelsIncludedBy(int? id)
        {
            return _db.Products
                .Include(product => product.ProductStrap).AsNoTracking()
                .Include(product => product.ProductColorDesign).AsNoTracking()
                .Include(product => product.ProductPrices).AsNoTracking()
                .Include(product => product.ProductDivision)
                    .ThenInclude(pd => pd.ProductKind).AsNoTracking()
                .Include(product => product.ProductDivision)
                    .ThenInclude(pd => pd.ProductMaterial).AsNoTracking()
                .Include(product => product.ProductCustomers).AsNoTracking()
                .Where(p => p.Id == id)
                .FirstOrDefault();
        }

        public void RemoveProduct(ProductModel product)
        {
            _db.Products.Remove(product);
            _db.ProductColorDesigns.Remove(product.ProductColorDesign);
            _db.ProductStraps.Remove(product.ProductStrap);
            _db.ProductPrices.Remove(product.ProductPrices);
            _db.SaveChanges();
        }

        public ProductDivisionModel GetProductDivisionWithChildModelsIncludedBy(int? id)
        {
            var productDivision = _db.ProductDivisions
                .Include(pd => pd.ProductKind).AsNoTracking()
                .Include(pd => pd.ProductMaterial).AsNoTracking()
                .Where(pd => pd.Id == id)
                .FirstOrDefault();
            return productDivision;
        }

        public void UpdateProduct(ProductModel updatedProduct)
        {
            _db.Update(updatedProduct.ProductColorDesign);
            _db.Update(updatedProduct.ProductStrap);
            _db.Update(updatedProduct.ProductPrices);
            _db.Products.Update(updatedProduct);
            _db.SaveChanges();
        }

        public string GetProductDivisionNameBy(int productDivisionId)
        {
            return GetAllProductDivisions()
                .Where(pd => pd.Id == productDivisionId)
                .Select(pd => pd.Name).FirstOrDefault();
        }

        public IEnumerable<ProductDivisionModel> GetAllProductDivisionsWithChildModelsIncluded()
        {
            IEnumerable<ProductDivisionModel> productDivisions = _db.ProductDivisions
                    .Include(pd => pd.ProductKind).AsNoTracking()
                    .Include(pd => pd.ProductMaterial)
                    .AsNoTracking();
            return productDivisions;
        }

        public void AddProductDivision(ProductDivisionModel productDivision)
        {
            _db.Add(productDivision);
            _db.SaveChanges();
        }

        public void RemoveProductDivision(ProductDivisionModel productDivision)
        {
            _db.ProductDivisions.Remove(productDivision);
            _db.ProductMaterials.Remove(productDivision.ProductMaterial);
            _db.ProductKinds.Remove(productDivision.ProductKind);
            _db.SaveChanges();
        }

        public void UpdateProductDivision(ProductDivisionModel productDivision)
        {
            _db.Update(productDivision);
            _db.SaveChanges();
        }

        public IEnumerable<ProductModel> GetProductsBy(ProductDivisionModel productDivision)
        {
            return _db.Products.AsNoTracking()
                .Where(p => p.ProductDivision == productDivision);
        }

        public ProductImageModel GetProductImageBy(int? productId)
        {
            var product = _db.Products
                .Include(p => p.ProductImage)
                .Where(p => p.Id == productId).FirstOrDefault();
            return product.ProductImage;
        }

        public ProductModel GetProductBy(int id)
        {
            return _db.Products.AsNoTracking().Where(p => p.Id == id).FirstOrDefault();
        }

        public void UpdateRangeOfProducts(IEnumerable<ProductModel> editedProducts)
        {
            _db.Products.UpdateRange(editedProducts);
            _db.SaveChanges();
        }
    }
}
