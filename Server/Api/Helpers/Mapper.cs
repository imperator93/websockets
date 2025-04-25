using AutoMapper;
using Api.Dto;
using Api.Models;

namespace Api.Helpers;

public class Mapper : Profile
{
    public Mapper()
    {
        CreateMap<User, UserResponse>();
        CreateMap<UserRequest, User>();
    }
}