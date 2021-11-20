using Caliburn.Micro;
using DADesktopUI.Library.Api;
using DADesktopUI.Library.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace DADesktopUI.ViewModels
{
    public class ProductsViewModel : Screen
    {
        private BindableCollection<ProductModel> _products;
        private IProductEndpoint _productEndpoint;
        private StatusInfoViewModel _status;
        private IWindowManager _window;
        public ProductsViewModel(IProductEndpoint productEndpoint, StatusInfoViewModel statusInfoViewModel, IWindowManager window)
        {
            _productEndpoint = productEndpoint;
            _status = statusInfoViewModel;
            _window = window;
        }
        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            try
            {
                await LoadProducts();
                //throw new NotImplementedException("Testing error...");
            }
            catch (Exception ex)
            {
                dynamic settings = new ExpandoObject();
                settings.WindowtartupLocation = WindowStartupLocation.CenterOwner;
                settings.ResizeMode = ResizeMode.NoResize;
                settings.Title = "System Error";
                if (ex.Message == "Unauthorized")
                {
                    _status.UpdateMessage("Unauthorized access", "You are not authorized to access this.");
                    await _window.ShowWindowAsync(_status, null, settings);
                }
                else
                {
                    _status.UpdateMessage("Fatal Error", ex.StackTrace);
                    await _window.ShowWindowAsync(_status, null, settings);
                }
                await TryCloseAsync();
            }
           
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
                if (_selectedProduct is not null)
                {
                    SetCopyProductControls();
                }
                
                NotifyOfPropertyChange(() => CanCopyProduct);
            }
        }
        private string _designation;
        public string ProductDesignation
        {
            get { return _designation; }
            set
            {
                _designation = value;
                NotifyOfPropertyChange(() => ProductDesignation);
                NotifyOfPropertyChange(() => CanCopyProduct);
            }
        }
        private long _ean;

        public long ProductEAN
        {
            get { return _ean; }
            set
            {
                _ean = value;
                NotifyOfPropertyChange(() => ProductEAN);
                NotifyOfPropertyChange(() => CanCopyProduct);
            }
        }

        private string _productName;

        public string ProductName
        {
            get { return _productName; }
            set 
            {
                _productName = value;
                NotifyOfPropertyChange(() => ProductName);
                NotifyOfPropertyChange(() => CanCopyProduct);
            }
        }

        private string _productType;

        public string ProductType
        {
            get { return _productType; }
            set 
            { 
                _productType = value;
                NotifyOfPropertyChange(() => ProductType);
                NotifyOfPropertyChange(() => CanCopyProduct);
            }
        }


        private void SetCopyProductControls()

        {
            ProductName = _selectedProduct.Name;
            NotifyOfPropertyChange(() => ProductName);
            ProductType = _selectedProduct.Type;
            NotifyOfPropertyChange(() => ProductType);
        }
        public bool CanCopyProduct
        {
            get
            {
                bool output = _selectedProduct is not null &&
                    !string.IsNullOrEmpty(ProductDesignation) &&
                    !string.IsNullOrEmpty(ProductEAN.ToString()) &&
                    !string.IsNullOrEmpty(ProductName) &&
                    !string.IsNullOrEmpty(ProductType);

                return output;
            }
        }

        public async Task CopyProduct()
        {
            ProductModel newProduct = _selectedProduct;
            newProduct.Designation = ProductDesignation;
            newProduct.EAN=ProductEAN;
            newProduct.Name = ProductName;
            newProduct.Type = ProductType;
            await _productEndpoint.PostProduct(newProduct);
            Products.Add(newProduct);
            EraseCopyProductControls();
        }

        private void EraseCopyProductControls()
        {
            ProductDesignation = "";
            ProductName = "";
            ProductEAN = 1111111111111;
            ProductType = "";

        }
    }
}
