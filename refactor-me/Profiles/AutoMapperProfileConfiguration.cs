using AutoMapper;
using refactor_me.Domain;
using refactor_me.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace refactor_me
{
    public class AutoMapperProfileConfiguration : Profile
    {
        public AutoMapperProfileConfiguration()
        {
            CreateMap<Product, ProductViewModel>();
            CreateMap<ProductViewModel, Product>();
            CreateMap<ProductOption, ProductOptionViewModel>();
            CreateMap<ProductOptionViewModel, ProductOption>();
            CreateMap<IEnumerable<Product>, ProductItemsViewModel>()
                .ForMember(a => a.Items, opt => opt.MapFrom(src => src));
            CreateMap<IEnumerable<ProductOption>, ProductOptionItemsViewModel>()
                .ForMember(a => a.Items, opt => opt.MapFrom(src => src)); ;
        }
    }
}