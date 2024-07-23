using AutoMapper;
using tester.DTOs;
using tester.Models;

namespace tester.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User,UserRequestDTO>().ReverseMap();

            CreateMap<CreateUserRequestDTO,User>().ReverseMap();
        }
    }
}
