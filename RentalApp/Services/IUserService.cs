using RentalApp.Models;

namespace RentalApp.Services;

public interface IUserService
{
    User AddUser(User user);
    User? FindById(int id);
    List<User> GetAllUsers();
}