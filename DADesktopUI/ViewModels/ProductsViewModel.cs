using Caliburn.Micro;
using DADesktopUI.Library.Api;
using DADesktopUI.Library.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DADesktopUI.ViewModels
{
    public class ProductsViewModel : Screen
    {
        private BindableCollection<ProductModel> _products;
        private IProductEndpoint _productEndpoint;
        private ICollectionView _fooView;
        public ProductsViewModel(IProductEndpoint productEndpoint)
        {
            _productEndpoint = productEndpoint;
        }
        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            await LoadProducts();
        }
        private async Task LoadProducts()
        {
            var productList = await _productEndpoint.GetAll();
            Products = new BindableCollection<ProductModel>(productList);
        }
        public BindableCollection<ProductModel> Products
        {
            get { return _products; }
            set 
            {
                _products = value; 
                NotifyOfPropertyChange(() => Products);
            }
        }
        private ProductModel _selectedProduct;

        public ProductModel SelectedProduct
        {
            get { return _selectedProduct; }
            set
            {
                _selectedProduct = value;
                SetCopyProductControls();
            }
        }
        public string ProductName { get; set; }
        public string ProductType { get; set; }

        private void SetCopyProductControls()

        {
            ProductName = _selectedProduct.Name;
            NotifyOfPropertyChange(() => ProductName);
            ProductType = _selectedProduct.Type;
            NotifyOfPropertyChange(() => ProductType);
        }
    }
}
