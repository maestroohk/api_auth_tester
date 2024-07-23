using AutoMapper;
using tester.DTOs.Auth;
using tester.DTOs.Products;
using tester.Models;

namespace tester.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User,UserRequestDTO>().ReverseMap();

            CreateMap<CreateUserRequestDTO,User>().ReverseMap();

            CreateMap<CreateProductDTO, Product>().ReverseMap();

            CreateMap<ProductDTO, Product>().ReverseMap();

            CreateMap<UpdateProductDTO, Product>().ReverseMap();
        }
    }
}
