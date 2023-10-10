namespace VebTech.UserManagement.WebApi.Helpers;

using EntityFramework.Entities;
using WebApi.Models;

using AutoMapper;

public class Mapper : Profile
{
    public Mapper()
    {
        CreateMap<UserRequest, User>();
    }
}