using DADataManager.Library.DataAccess;
using DADataManager.Library.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace DADataManager.Library.DataAccess
{
    public class ProductData : IProductData
    {
        private readonly ISqlDataAccess _sql;

        public ProductData(ISqlDataAccess sqlDataAccess)
        {
            _sql = sqlDataAccess;
        }

        public List<ProductModel> GetProducts()
        {
            //var output = sql.LoadData<ProductModel, dynamic>("dbo.spProduct_GetAll", new { }, "DAData");
            var output = _sql.LoadData<ProductModel, dynamic>("dbo.spProduct_GetAll", new { }, "DADataConnection");

            return output;
        }
    }
}