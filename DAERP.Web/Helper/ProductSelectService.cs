using DAERP.BL.Models.Product;
using DAERP.DAL.DataAccess;
using DAERP.Web.ViewModels;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAERP.Web.Helper
{
    public class ProductSelectService : IProductSelectService
    {
        private readonly IProductData _productData;
        private ITempDataDictionary _tempData;
        private List<SelectedProduct> _selectedProducts;
        public ProductSelectService(IProductData productData)
        {
            _productData = productData;
        }
        public List<SelectedProduct> Get(ITempDataDictionary tempData)
        {
            _tempData = tempData;
            RetrieveSelectedProduct();
            return _selectedProducts;
        }
        public List<SelectedProduct> Get(int? addSelected,
            int? removeSelected,
            int? removeAllSelected, ITempDataDictionary tempData, bool checkStockRemain)
        {
            _tempData = tempData;
            RetrieveSelectedProduct();
            if (addSelected is not null)
            {
                AddSelected(addSelected);
                if (checkStockRemain)
                {
                    CheckStockRemain();
                }
            }
            if (removeSelected is not null)
            {
                RemoveSelected(removeSelected);
            }
            if (removeAllSelected is not null)
            {
                RemoveAllSelected(removeAllSelected);
            }
            return _selectedProducts;
        }

        private void CheckStockRemain()
        {
            _selectedProducts.ForEach(sp => sp.IsPossibleAdd = sp.Amount < sp.Product.MainStockAmount);
        }

        private void RetrieveSelectedProduct()
        {
            _selectedProducts = new List<SelectedProduct>();
            var selectedProductsIds = _tempData["SelectedProductsIds"] as int[];
            var selectedProductAmounts = _tempData["SelectedProductAmounts"] as int[];
            if (selectedProductsIds is not null)
            {
                for (int i = 0; i < selectedProductsIds.Length; i++)
                {
                    int id = selectedProductsIds[i];
                    int amount = selectedProductAmounts[i];
                    ProductModel product = _productData.GetProductBy(id);
                    SelectedProduct selectedProduct = new SelectedProduct()
                    {
                        Product = product,
                        Amount = amount
                    };
                    _selectedProducts.Add(selectedProduct);
                }
            }
            _tempData.Clear();
        }

        private void AddSelected(int? addSelected)
        {
            if (_selectedProducts.Select(sp => sp.Product.Id).ToList().Contains((int)addSelected))
            {
                _selectedProducts.Where(sp => sp.Product.Id == (int)addSelected).FirstOrDefault().Amount += 1;
            }
            else
            {
                ProductModel product = _productData.GetProductBy(addSelected);
                _selectedProducts.Add(new SelectedProduct()
                {
                    Product = product,
                    Amount = 1
                });
            }
        }
        private void RemoveSelected(int? removeSelected)
        {
            if (_selectedProducts.Select(sp => sp.Product.Id).ToList().Contains((int)removeSelected))
            {
                SelectedProduct selectedProduct = _selectedProducts.Where(sp => sp.Product.Id == (int)removeSelected).FirstOrDefault();
                if (_selectedProducts.Where(sp => sp.Product.Id == (int)removeSelected).FirstOrDefault().Amount == 1)
                {
                    _selectedProducts.Remove(selectedProduct);
                }
                else
                {
                    _selectedProducts.Where(sp => sp.Product.Id == (int)removeSelected).FirstOrDefault().Amount -= 1;
                }
            }
            else
            {
                // TODO: add error
            }
        }
        private void RemoveAllSelected(int? removeAllSelected)
        {
            if (_selectedProducts.Select(sp => sp.Product.Id).ToList().Contains((int)removeAllSelected))
            {
                SelectedProduct selectedProduct = _selectedProducts.Where(sp => sp.Product.Id == (int)removeAllSelected).FirstOrDefault();
                _selectedProducts.Remove(selectedProduct);
            }
            else
            {
                // TODO: add error
            }
        }
    }
}
