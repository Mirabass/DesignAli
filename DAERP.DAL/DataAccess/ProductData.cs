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
                .Include(product => product.ProductStrap)
                .Include(product => product.ProductColorDesign)
                .Include(product => product.ProductDivision)
                    .ThenInclude(pd => pd.ProductKind)
                .Include(product => product.ProductDivision)
                    .ThenInclude(pd => pd.ProductMaterial);
            return products;
        }
        public IEnumerable<ProductDivisionModel> GetAllProductDivisions()
        {
            return _db.ProductDivisions;
        }

        public ProductDivisionModel GetProductDivisionBy(int productDivisionId)
        {
            ProductDivisionModel productDivision = _db.ProductDivisions.AsNoTracking()
                .Include(pd => pd.ProductKind)
                .Include(pd => pd.ProductMaterial)
                .Where(pd => pd.Id == productDivisionId)
                .FirstOrDefault();
            return productDivision;
        }

        public void AddProduct(ProductModel product)
        {
            _db.Add(product);
            _db.Entry(product.ProductDivision).State = EntityState.Unchanged;
            _db.Entry(product.ProductDivision.ProductKind).State = EntityState.Unchanged;
            _db.Entry(product.ProductDivision.ProductMaterial).State = EntityState.Unchanged;
            _db.SaveChanges();
        }

        public ProductModel GetProductBy(int? id)
        {
            return _db.Products.Where(p => p.Id == id).Include(p => p.ProductDivision).FirstOrDefault();
        }

        public ProductModel GetProductWithChildModelsIncludedBy(int? id)
        {
            return _db.Products.AsNoTracking().Where(p => p.Id == id)
                .Include(product => product.ProductStrap)
                .Include(product => product.ProductColorDesign)
                .Include(product => product.ProductDivision)
                    .ThenInclude(pd => pd.ProductKind)
                .Include(product => product.ProductDivision)
                    .ThenInclude(pd => pd.ProductMaterial)
                .FirstOrDefault();
        }

        public void RemoveProduct(ProductModel product)
        {
            _db.ProductColorDesigns.Remove(product.ProductColorDesign);
            _db.ProductStraps.Remove(product.ProductStrap);
            _db.Products.Remove(product);
            _db.SaveChanges();
        }

        public ProductDivisionModel GetProductDivisionWithChildModelsIncludedBy(int? id)
        {
            var productDivision = _db.ProductDivisions.AsNoTracking().Where(pd => pd.Id == id)
                .Include(pd => pd.ProductKind)
                .Include(pd => pd.ProductMaterial)
                .FirstOrDefault();
            return productDivision;
        }

        public void UpdateProduct(ProductModel updatedProduct)
        {
            _db.Update(updatedProduct.ProductColorDesign);
            _db.Update(updatedProduct.ProductStrap);
            _db.Products.Update(updatedProduct);
            _db.SaveChanges();
        }

        public string GetProductDivisionNameBy(int productDivisionId)
        {
            return GetAllProductDivisions().Where(pd => pd.Id == productDivisionId).Select(pd => pd.Name).FirstOrDefault();
        }

        public IEnumerable<ProductDivisionModel> GetAllProductDivisionsWithChildModelsIncluded()
        {
            IEnumerable<ProductDivisionModel> productDivisions = _db.ProductDivisions
                    .Include(pd => pd.ProductKind)
                    .Include(pd => pd.ProductMaterial);
            return productDivisions;
        }

        public void AddProductDivision(ProductDivisionModel productDivision)
        {
            _db.Add(productDivision);
            _db.SaveChanges();
        }

        public void RemoveProductDivision(ProductDivisionModel productDivision)
        {
            _db.ProductMaterials.Remove(productDivision.ProductMaterial);
            _db.ProductKinds.Remove(productDivision.ProductKind);
            _db.ProductDivisions.Remove(productDivision);
            _db.SaveChanges();
        }
    }
}
