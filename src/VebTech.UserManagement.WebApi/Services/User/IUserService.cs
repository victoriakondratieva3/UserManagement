namespace VebTech.UserManagement.WebApi.Services.User;

using Filters;
using Models;

public interface IUserService
{
    Task<IEnumerable<User>> GetAllAsync(PaginationFilter paginationFilter, Filter filter, string sortOrder);
    Task<User> GetByIdAsync(int id);
    Task UpdateAsync(UserRequest model, int id);
    Task<User> CreateAsync(UserRequest model);
    Task<User> AddRoleAsync(int userId, int roleId);
    Task DeleteAsync(int id);
    bool IsValidUserData(LoginRequest model);
}
