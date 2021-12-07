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
        public (int,int,int) AddProduct(ProductModel product)
        {
            int productColorDesignId = product.ProductColorDesign.Id;
            int productDivisionId = product.ProductDivision.Id;
            int productStrapId = product.ProductStrap.Id;
            var productToInsert = new DynamicParameters();
            productToInsert.Add("Id",dbType:DbType.Int32,direction: ParameterDirection.Output);
            productToInsert.Add("Designation",product.Designation);
            productToInsert.Add("Motive",product.Motive);
            productToInsert.Add("EAN",product.EAN);
            productToInsert.Add("Accessories",product.Accessories);
            productToInsert.Add("Design",product.Design);
            productToInsert.Add("ProductColorDesignId",productColorDesignId);
            productToInsert.Add("ProductDivisionId",productDivisionId);
            productToInsert.Add("ProductStrapId",productStrapId);
            productToInsert.Add("NewProductColorDesignId", dbType: DbType.Int32, direction: ParameterDirection.Output);
            productToInsert.Add("NewProductStrapId", dbType: DbType.Int32, direction: ParameterDirection.Output);
            using (IDbConnection connection = new SqlConnection(_config.GetConnectionString("DADataConnection")))
            {
                connection.Query<int>("dbo.spProduct_Insert", productToInsert,
                    commandType: CommandType.StoredProcedure);
                int NewProductId = productToInsert.Get<int>("Id");
                int NewColorDesignId = productToInsert.Get<int>("NewProductColorDesignId");
                int NewStrapId = productToInsert.Get<int>("NewProductStrapId");
                return (NewProductId,NewColorDesignId,NewStrapId);
            }
        }

        public void DeleteProduct(int Id)
        {
            _sql.SaveData("dbo.spProduct_Delete", new { Id = Id }, "DADataConnection");
        }

        public void Update(ProductModel product)
        {
            object productToUpdate = new
            {
                Id = product.Id,
                Designation = product.Designation,
                Motive = product.Motive,
                EAN = product.EAN,
                Accessories = product.Accessories,
                Design = product.Design,
                ColorDesignQuantity = product.ProductColorDesign.Quantity,
                ColorDesignOrientation = product.ProductColorDesign.Orientation,
                ColorDesignMainPartRAL = product.ProductColorDesign.MainPartRAL,
                ColorDesignMainPartColorName = product.ProductColorDesign.MainPartColorName,
                ColorDesignPocketColorName = product.ProductColorDesign.PocketColorName,
                ColorDesignPocketRAL = product.ProductColorDesign.PocketRAL,
                StrapType = product.ProductStrap.Type,
                StrapMaterial = product.ProductStrap.Material,
                StrapLength = product.ProductStrap.Length,
                StrapWidth = product.ProductStrap.Width,
                StrapRAL = product.ProductStrap.RAL,
                StrapColorName = product.ProductStrap.ColorName,
                StrapAttachment = product.ProductStrap.Attachment
            };
            _sql.SaveData("dbo.spProduct_Update", productToUpdate, "DADataConnection");
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