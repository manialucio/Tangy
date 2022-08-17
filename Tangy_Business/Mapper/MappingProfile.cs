using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tangy_DataAccess;
using Tangy_DataAccess.ViewModel;
using Tangy_Models;

namespace Tangy_Business.Mapper
{
    public class MappingProfile: Profile
    {

        public MappingProfile()
        {
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<ProductPrice, ProductPriceDto>().ReverseMap();
            CreateMap<Product, ProductDto>().ForMember(d => d.Prices, map => map.MapFrom(src => src.ProductPrices));
            CreateMap<ProductDto, Product>().ForMember(d => d.ProductPrices, map => map.MapFrom(src => src.Prices));
            CreateMap<OrderHeader, OrderHeaderDto>().ReverseMap();
            CreateMap<OrderDetail, OrderDetailDto>().ReverseMap();
            CreateMap<Order, OrderDto>().ReverseMap();


        }
    }
}
