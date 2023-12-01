using AutoMapper;
using UserManagement.EntityFramework.Entities;
using UserManagement.WebApi.Models;

namespace UserManagement.WebApi.Helpers;

public class Mapper : Profile
{
    public Mapper()
    {
        CreateMap<UserRequest, User>();
    }
}