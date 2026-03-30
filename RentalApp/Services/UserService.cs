using RentalApp.Data;
using RentalApp.Models;

namespace RentalApp.Services;

public class UserService : IUserService
{
    private readonly RentalDbContext _context;

    public UserService(RentalDbContext context)
    {
        _context = context;
    }

    public User AddUser(User user)
    {
        user.Id = _context.GenerateUserId();
        _context.Users.Add(user);
        return user;
    }

    public User? FindById(int id) => _context.Users.FirstOrDefault(u => u.Id == id);

    public List<User> GetAllUsers() => _context.Users.ToList();
}