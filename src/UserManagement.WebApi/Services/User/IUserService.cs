using UserManagement.WebApi.Filters;
using UserManagement.WebApi.Models;
using E = UserManagement.EntityFramework.Entities;

namespace UserManagement.WebApi.Services.User;

public interface IUserService
{
    Task<IEnumerable<E.User>> GetAllAsync(PaginationFilter paginationFilter, Filter filter, string sortOrder);
    Task<E.User> GetByIdAsync(int id);
    Task UpdateAsync(UserRequest request, int id);
    Task<E.User> CreateAsync(UserRequest request);
    Task<E.User> AddRoleAsync(int userId, int roleId);
    Task DeleteAsync(int id);
    bool IsValidUserData(LoginRequest request);
}
