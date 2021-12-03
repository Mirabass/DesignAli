﻿using DADesktopUI.Library.Models.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Json;

namespace DADesktopUI.Library.Api
{
    public class ProductEndpoint : IProductEndpoint
    {
        private IAPIHelper _apiHelper;
        public ProductEndpoint(IAPIHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task DeleteProduct(ProductModel product)
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.DeleteAsync($"/api/Product?id={product.Id}"))
            {
                if (response.IsSuccessStatusCode)
                {
                    // Say OK.
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<List<ProductModel>> GetAll()
        {
            //var products = await _apiHelper.ApiClient.GetFromJsonAsync<List<ProductModel>>("/api/Product");
            //return products;
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("/api/Product"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<ProductModel>>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<List<ProductDivisionModel>> GetDivisions()
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("/api/ProductDivision"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<ProductDivisionModel>>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task PostProduct(ProductModel product)
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.PostAsJsonAsync("/api/Product", product))
            {
                if (response.IsSuccessStatusCode)
                {
                    // Say OK.
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task UpdateProduct(ProductModel product)
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.PutAsJsonAsync("/api/Product", product))
            {
                if (response.IsSuccessStatusCode)
                {
                    // Say OK.
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}
