using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UserManagement.EntityFramework.Helpers;
using UserManagement.WebApi.Filters;
using UserManagement.WebApi.Helpers;
using UserManagement.WebApi.Models;
using E = UserManagement.EntityFramework.Entities;

namespace UserManagement.WebApi.Services.User;

public class UserService : IUserService
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public UserService(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<E.User>> GetAllAsync(PaginationFilter paginationFilter, Filter filter, string sortOrder)
    {
        var users = _context.Users.Include(u => u.Roles).AsQueryable();

        if (!string.IsNullOrEmpty(filter.RoleName))
        {
            users = users.Where(u => u.Roles.Any(r => r.Name.ToLower().Contains(filter.RoleName.ToLower())));
        }

        if (!string.IsNullOrEmpty(filter.Name))
        {
            users = users.Where(u => u.Name.ToLower().Contains(filter.Name.ToLower()));
        }

        if (!string.IsNullOrEmpty(filter.Email))
        {
            users = users.Where(u => u.Email.ToLower().Contains(filter.Email.ToLower()));
        }

        if (filter.MinAge > 0)
        {
            users = users.Where(u => u.Age >= filter.MinAge);
        }

        if (filter.MaxAge > 0)
        {
            users = users.Where(u => u.Age <= filter.MaxAge);
        }

        switch (sortOrder)
        {
            case Constants.NameAsc:
                users = users.OrderBy(u => u.Name);
                break;
            case Constants.NameDesc:
                users = users.OrderByDescending(u => u.Name);
                break;
            case Constants.AgeAsc:
                users = users.OrderBy(u => u.Age);
                break;
            case Constants.AgeDesc:
                users = users.OrderByDescending(u => u.Age);
                break;
            case Constants.EmailAsc:
                users = users.OrderBy(u => u.Email);
                break;
            case Constants.EmailDesc:
                users = users.OrderByDescending(u => u.Email);
                break;
        }

        var validPaginationFilter = new PaginationFilter(paginationFilter.PageNumber, paginationFilter.PageSize);

        return await users
               .Skip((validPaginationFilter.PageNumber - 1) * validPaginationFilter.PageSize)
               .Take(validPaginationFilter.PageSize)
               .ToListAsync();
    }

    public async Task<E.User> GetByIdAsync(int id)
    {
        return await _context.Users
            .Include(u => u.Roles)
            .FirstOrDefaultAsync(u => u.Id == id)
                ?? throw new HttpException(StatusCodes.Status404NotFound, $"User with id:{id} not found.");
    }

    public async Task UpdateAsync(UserRequest request, int id)
    {
        if (request.Id != id)
        {
            throw new HttpException(StatusCodes.Status400BadRequest, 
                "IDs don't match.");
        }

        if (!UserExists(id))
        {
            throw new HttpException(StatusCodes.Status404NotFound, 
                $"User with id:{id} not found.");
        }

        var emailExists = _context.Users.Any(u => u.Email == request.Email && u.Id != id);
        if (emailExists)
        {
            throw new HttpException(StatusCodes.Status500InternalServerError, 
                $"User with email:{request.Email} already exists.");
        }

        var user = _mapper.Map<E.User>(request);

        _context.Entry(user).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            throw;
        }
    }

    public async Task<E.User> CreateAsync(UserRequest request)
    {
        if (UserExists(request.Id))
        {
            throw new HttpException(StatusCodes.Status500InternalServerError, 
                "User with this id already exists.");
        }

        if (EmailExists(request.Email))
        {
            throw new HttpException(StatusCodes.Status500InternalServerError, 
                "User with this email already exists.");
        }

        var user = _mapper.Map<E.User>(request);

        var userRole = await _context.Roles.FindAsync(Constants.UserRoleId);
        user.Roles = new List<E.Role>() { userRole };

        _context.Users.Add(user);

        await _context.SaveChangesAsync();

        return user;
    }

    public async Task<E.User> AddRoleAsync(int userId, int roleId)
    {
        var user = await _context.Users
            .Include(u => u.Roles)
            .FirstOrDefaultAsync(u => u.Id == userId)
            ?? throw new HttpException(StatusCodes.Status404NotFound, 
                $"User with id:{userId} not found.");

        var role = await _context.Roles
            .FirstOrDefaultAsync(u => u.Id == roleId)
            ?? throw new HttpException(StatusCodes.Status404NotFound, 
                $"Role with id:{roleId} not found.");

        if (user.Roles.Contains(role))
        {
            throw new HttpException(StatusCodes.Status500InternalServerError, 
                "This Role already exists for the User.");
        }

        user.Roles.Add(role);

        _context.Entry(user).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();

            return user;
        }
        catch (DbUpdateConcurrencyException)
        {
            throw;
        }
    }

    public async Task DeleteAsync(int id)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == id)
            ?? throw new HttpException(StatusCodes.Status404NotFound, 
                $"User with id:{id} not found.");

        _context.Users.Remove(user);

        await _context.SaveChangesAsync();
    }

    public bool IsValidUserData(LoginRequest request)
    {
        return (_context.Users?.Any(e => e.Email == request.Email)).GetValueOrDefault();
    }

    private bool EmailExists(string email)
    {
        return (_context.Users?.Any(e => e.Email == email)).GetValueOrDefault();
    }

    private bool UserExists(int id)
    {
        return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}
