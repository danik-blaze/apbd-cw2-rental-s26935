using RentalApp.Data;
using RentalApp.Models;

namespace RentalApp.Services;

public class RentalService : IRentalService
{
    private readonly RentalDbContext _context;
    private readonly IEquipmentService _equipmentService;
    private readonly IUserService _userService;
    private const decimal LateFeePerDay = 5.0m;

    public RentalService(RentalDbContext context, IEquipmentService equipmentService, IUserService userService)
    {
        _context = context;
        _equipmentService = equipmentService;
        _userService = userService;
    }

    public Rental RentEquipment(int equipmentId, int userId, int rentalDays)
    {
        var equipment = _equipmentService.FindById(equipmentId);
        if (equipment == null)
            throw new ArgumentException("Sprzęt nie istnieje");

        if (!equipment.IsAvailable)
            throw new InvalidOperationException("Sprzęt jest niedostępny");

        var user = _userService.FindById(userId);
        if (user == null)
            throw new ArgumentException("Użytkownik nie istnieje");

        var activeRentals = GetActiveRentalsByUser(userId);
        if (activeRentals.Count >= user.MaxActiveRentals)
            throw new InvalidOperationException($"Użytkownik przekroczył limit wypożyczeń ({user.MaxActiveRentals})");

        equipment.IsAvailable = false;
        equipment.Status = "rented";

        var rental = new Rental(equipment, user, DateTime.Now, rentalDays);
        rental.Id = _context.GenerateRentalId();
        _context.Rentals.Add(rental);
        return rental;
    }

    public Rental? ReturnEquipment(int rentalId, DateTime returnDate)
    {
        var rental = _context.Rentals.FirstOrDefault(r => r.Id == rentalId);
        if (rental == null || !rental.IsActive)
            return null;

        rental.ActualReturnDate = returnDate;
        var lateFee = rental.CalculateLateFee(returnDate, LateFeePerDay);
        rental.LateFee = lateFee;

        rental.Equipment.IsAvailable = true;
        rental.Equipment.Status = "available";

        return rental;
    }

    public List<Rental> GetActiveRentalsByUser(int userId)
    {
        return _context.Rentals
            .Where(r => r.User.Id == userId && r.IsActive)
            .ToList();
    }

    public List<Rental> GetOverdueRentals(DateTime currentDate)
    {
        return _context.Rentals
            .Where(r => r.IsActive && r.ExpectedReturnDate < currentDate)
            .ToList();
    }

    public string GenerateReport()
    {
        var sb = new System.Text.StringBuilder();
        sb.AppendLine("=== RAPORT WYPOŻYCZALNI ===");
        sb.AppendLine($"Data: {DateTime.Now:yyyy-MM-dd HH:mm}");
        sb.AppendLine();
        sb.AppendLine("Sprzęt:");
        foreach (var eq in _equipmentService.GetAllEquipment())
            sb.AppendLine($"  {eq}");
        sb.AppendLine();
        sb.AppendLine("Użytkownicy:");
        foreach (var user in _userService.GetAllUsers())
            sb.AppendLine($"  {user}");
        sb.AppendLine();
        sb.AppendLine("Aktywne wypożyczenia:");
        foreach (var rental in _context.Rentals.Where(r => r.IsActive))
            sb.AppendLine($"  ID {rental.Id}: {rental.Equipment.Name} -> {rental.User.FullName} do {rental.ExpectedReturnDate:yyyy-MM-dd}");
        sb.AppendLine();
        sb.AppendLine("Przeterminowane wypożyczenia:");
        foreach (var rental in GetOverdueRentals(DateTime.Now))
            sb.AppendLine($"  ID {rental.Id}: {rental.Equipment.Name} -> {rental.User.FullName}, oczekiwany zwrot {rental.ExpectedReturnDate:yyyy-MM-dd}");
        return sb.ToString();
    }
}