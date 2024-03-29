﻿using AutoMapper;
using DAERP.BL.Models.Product;

namespace DAERP.Web.ViewModels.Mapping
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductCreateViewModel, ProductModel>();
            CreateMap<ProductModel, ProductCreateViewModel>();
        }
    }
}
