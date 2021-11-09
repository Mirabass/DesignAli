using DADataManager.Library.Internal.DataAccess;
using DADataManager.Library.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace DADataManager.Library.DataAccess
{
    public class ProductData
    {
        private readonly IConfiguration _config;

        public ProductData(IConfiguration config)
        {
            _config = config;
        }

        public List<ProductModel> GetProducts()
        {
            SqlDataAccess sql = new SqlDataAccess(_config);
            //var output = sql.LoadData<ProductModel, dynamic>("dbo.spProduct_GetAll", new { }, "DAData");
            var output = sql.LoadData<ProductModel, dynamic>("dbo.spProduct_GetAll", new { }, "DADataConnection");

            return output;
        }
    }
}