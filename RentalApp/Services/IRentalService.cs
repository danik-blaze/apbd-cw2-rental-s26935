using RentalApp.Models;

namespace RentalApp.Services;

public interface IRentalService
{
    Rental RentEquipment(int equipmentId, int userId, int rentalDays);
    Rental? ReturnEquipment(int rentalId, DateTime returnDate);
    List<Rental> GetActiveRentalsByUser(int userId);
    List<Rental> GetOverdueRentals(DateTime currentDate);
    string GenerateReport();
}