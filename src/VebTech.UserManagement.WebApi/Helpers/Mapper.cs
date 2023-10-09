namespace VebTech.UserManagement.WebApi.Helpers;

using Models;

using AutoMapper;

public class Mapper : Profile
{
    public Mapper()
    {
        CreateMap<UserRequest, User>();
    }
}