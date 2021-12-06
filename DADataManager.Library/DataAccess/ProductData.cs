using DADataManager.Library.Models.Product;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace DADataManager.Library.DataAccess
{
    public class ProductData : IProductData
    {
        private readonly ISqlDataAccess _sql;
        private readonly IConfiguration _config;

        public ProductData(ISqlDataAccess sqlDataAccess, IConfiguration config)
        {
            _sql = sqlDataAccess;
            _config = config;
        }

        public List<ProductModel> GetProducts()
        {
            //var output = sql.LoadData<ProductModel, dynamic>("dbo.spProduct_GetAll", new { }, "DAData");
            //var output = _sql.LoadData<ProductModel, dynamic>("dbo.spProduct_GetAll", new { }, "DADataConnection");
            using (IDbConnection connection = new SqlConnection(_config.GetConnectionString("DADataConnection")))
            {
                var output = connection.Query<ProductModel, ProductDivisionModel,
                    ProductMaterialModel, ProductColorDesignModel, ProductKindModel,
                    ProductStrapModel, ProductModel>("dbo.spProduct_GetAll",
                    (product, division, material, colorDesign, kind, strap) =>
                    {
                        product.ProductDivision = division;
                        product.ProductDivision.ProductKind = kind;
                        product.ProductDivision.ProductMaterial = material;
                        product.ProductColorDesign = colorDesign;
                        product.ProductStrap = strap;
                        return product;
                    }
                    );
                return output.ToList();
            }
        }
        public List<ProductModel> GetProducts(string designation)
        {
            using (IDbConnection connection = new SqlConnection(_config.GetConnectionString("DADataConnection")))
            {
                var output = connection.Query<ProductModel, ProductDivisionModel,
                    ProductMaterialModel, ProductColorDesignModel, ProductKindModel,
                    ProductStrapModel, ProductModel>("dbo.spProduct_GetByDesignation",
                    map: (product, division, material, colorDesign, kind, strap) =>
                    {
                        product.ProductDivision = division;
                        product.ProductDivision.ProductKind = kind;
                        product.ProductDivision.ProductMaterial = material;
                        product.ProductColorDesign = colorDesign;
                        product.ProductStrap = strap;
                        return product;
                    },
                    param: new { Designation = designation },
                    commandType: CommandType.StoredProcedure
                    );
                return output.ToList();
            }
        }
        public int AddProduct(ProductModel product)
        {
            int productColorDesignId = product.ProductColorDesign.Id;
            int productDivisionId = product.ProductDivision.Id;
            int productStrapId = product.ProductStrap.Id;
            object productToInsert = new
            {
                product.Id,
                product.Designation,
                product.Motive,
                product.EAN,
                product.Accessories,
                product.Design,
                productColorDesignId,
                productDivisionId,
                productStrapId
            };
            using (IDbConnection connection = new SqlConnection(_config.GetConnectionString("DADataConnection")))
            {
                int output = connection.Query<int>("dbo.spProduct_Insert", productToInsert,
                    commandType: CommandType.StoredProcedure).Single();
                return output;
            }
        }

        public void DeleteProduct(int Id)
        {
            _sql.SaveData("dbo.spProduct_Delete", new { Id = Id }, "DADataConnection");
        }

        public void Update(ProductModel product)
        {
            _sql.SaveData("dbo.spProduct_Update", product, "DADataConnection");
        }

        public List<ProductDivisionModel> GetProductDivisions()
        {
            using (IDbConnection connection = new SqlConnection(_config.GetConnectionString("DADataConnection")))
            {
                var output = connection.Query<ProductDivisionModel,
                    ProductKindModel, ProductMaterialModel,
                    ProductDivisionModel>("dbo.spProductDivision_GetAll",
                    (division, kind, material) =>
                    {
                        division.ProductKind = kind;
                        division.ProductMaterial = material;
                        return division;
                    }
                    );
                return output.ToList();
            }
        }
    }
}