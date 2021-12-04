using Caliburn.Micro;
using DADesktopUI.EventModels;
using DADesktopUI.Library;
using DADesktopUI.Library.Api;
using DADesktopUI.Library.Models.Product;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using static DADesktopUI.Library.Enums;

namespace DADesktopUI.ViewModels
{
    public class ProductsViewModel : Screen
    {
        private BindableCollection<ProductModel> _products;
        private BindableCollection<ProductDivisionModel> _productDivisions;
        private IProductEndpoint _productEndpoint;
        private StatusInfoViewModel _status;
        private IEventAggregator _events;
        public ProductsViewModel(IProductEndpoint productEndpoint, StatusInfoViewModel statusInfoViewModel,
            IEventAggregator events)
        {
            _productEndpoint = productEndpoint;
            _status = statusInfoViewModel;
            _events = events;
        }
        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            try
            {
                await LoadProducts();
                await LoadProductDivisions();
                //throw new NotImplementedException("Testing error...");
            }
            catch (Exception ex)
            {
                if (ex.Message == "Unauthorized")
                {
                    _status.UpdateMessage("System Error","Unauthorized access", "You are not authorized to access this.");
                    await _status.ShowDialogAsync();
                }
                else
                {
                    _status.UpdateMessage("System Error","Fatal Error", ex.StackTrace);
                    await _status.ShowDialogAsync();
                }
                await TryCloseAsync();
                await _events.PublishOnUIThreadAsync(new GoToEvent(GoTo.Home), new CancellationToken());
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
        private async Task LoadProductDivisions()
        {
            var productDivisionList = await _productEndpoint.GetDivisions();
            ProductDivisions = new BindableCollection<ProductDivisionModel>(productDivisionList);
        }

        public BindableCollection<ProductDivisionModel> ProductDivisions
        {
            get { return _productDivisions; }
            set
            {
                _productDivisions = value;
                NotifyOfPropertyChange(() => ProductDivisions);
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
                    new Task(async () =>
                    {
                        await SetCopyProductControls();
                    }).Start();
                }
                NotifyOfPropertyChange(() => CanCopyProduct);
                NotifyOfPropertyChange(() => CanDeleteProduct);
            }
        }
        private ProductDivisionModel _selectedProductDivision;

        public ProductDivisionModel SelectedProductDivision
        {
            get { return _selectedProductDivision; }
            set
            {
                if (ProductDivisions.Select(x => x.Id).Contains(value.Id))
                {
                    _selectedProductDivision = ProductDivisions.Where(x => x.Id == value.Id).FirstOrDefault();
                    RefreshDesignation();
                    NotifyOfPropertyChange(() => SelectedProductDivision);
                    NotifyOfPropertyChange(() => CanCopyProduct);
                }
                else
                {
                    //This error would be unhandled, so just continue.
                    //throw new Exception("Product division can not be set because it is not in collection of all product divisions.");
                }
            }
        }

        public async Task OnCellEdit()
        {
            await UpdateSelectedProduct();
        }

        private async Task UpdateSelectedProduct()
        {
            try
            {
                await _productEndpoint.UpdateProduct(_selectedProduct);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Unauthorized")
                {
                    _status.UpdateMessage("System Error", ex.Message, "You are not allowed to edit this product.");
                }
                else
                {
                    _status.UpdateMessage("System Error", ex.Message, ex.StackTrace);
                }
                await _status.ShowDialogAsync();
            }
        }
        private int _newDesign;

        public int NewDesign
        {
            get { return _newDesign; }
            set
            {
                _newDesign = value;
                RefreshDesignation();
                NotifyOfPropertyChange(() => CanCopyProduct);
            }
        }

        private string _designation;
        public string NewDesignation
        {
            get { return _designation; }
            set
            {
                _designation = value;
                NotifyOfPropertyChange(() => NewDesignation);
                NotifyOfPropertyChange(() => CanCopyProduct);
            }
        }
        private void RefreshDesignation()
        {
            string newDivisionLZ = CustomOperations.LeadingZeros(_selectedProductDivision.Number, 3);
            string kindLZ = CustomOperations.LeadingZeros(_selectedProductDivision.ProductKind.Number, 3);
            string materialLZ = CustomOperations.LeadingZeros(_selectedProductDivision.ProductMaterial.Number, 3);
            string newDesignLZ = CustomOperations.LeadingZeros(_newDesign, 4);
            NewDesignation = $"{newDivisionLZ}-{kindLZ}-" +
                $"{materialLZ}/{newDesignLZ}";
        }
        private long _ean;

        public long NewEAN
        {
            get { return _ean; }
            set
            {
                _ean = value;
                RefreshDesignation();
                NotifyOfPropertyChange(() => NewEAN);
                NotifyOfPropertyChange(() => CanCopyProduct);
            }
        }

        private async Task SetCopyProductControls()

        {
            try
            {
                SelectedProductDivision = _selectedProduct.ProductDivision;
            }
            catch (Exception ex)
            {
                _status.UpdateMessage("System Error", ex.Message, ex.StackTrace);
                await _status.ShowDialogAsync();
            }
            
        }
        public bool CanCopyProduct
        {
            get
            {
                bool output = _selectedProduct is not null &&
                    NewDesign != 0 &&
                    !string.IsNullOrEmpty(NewEAN.ToString()) &&
                    SelectedProductDivision is not null &&
                    !ExistNewDesignation();

                return output;
            }
        }

        private bool ExistNewDesignation()
        {
            // TODO: make sure that proposed designation does not already exist
            return false;
        }

        public async Task CopyProduct()
        {

            try
            {
                ProductModel newProduct = _selectedProduct;
                newProduct.Designation = NewDesignation;
                newProduct.ProductDivision = SelectedProductDivision;
                newProduct.EAN = NewEAN;
                await _productEndpoint.PostProduct(newProduct);
                Products.Add(newProduct);
                EraseCopyProductControls();
            }
            catch (Exception ex)
            {
                if (ex.Message == "Unauthorized")
                {
                    _status.UpdateMessage("System Error", ex.Message, "You are not allowed to copy product.");
                }
                else
                {
                    _status.UpdateMessage("System Error", ex.Message, ex.StackTrace);
                }
                await _status.ShowDialogAsync();
            }
        }

        private void EraseCopyProductControls()
        {
            NewDesignation = "";
            SelectedProductDivision = null;
            NewEAN = 1111111111111;

        }

        public bool CanDeleteProduct
        {
            get
            {
                return _selectedProduct is not null; 
            }
        }

        public async Task DeleteProduct()
        {
            try
            {
                await _productEndpoint.DeleteProduct(_selectedProduct);
                Products.Remove(SelectedProduct);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Unauthorized")
                {
                    _status.UpdateMessage("System Error", ex.Message, "You are not allowed to delete product.");
                }
                else
                {
                    _status.UpdateMessage("System Error", ex.Message, ex.StackTrace);
                }
                await _status.ShowDialogAsync();
            }
        }
    }
}
