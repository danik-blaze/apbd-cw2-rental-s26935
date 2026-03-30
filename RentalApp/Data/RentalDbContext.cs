using RentalApp.Models;

namespace RentalApp.Data;

public class RentalDbContext
{
    private static int _nextEquipmentId = 1;
    private static int _nextUserId = 1;
    private static int _nextRentalId = 1;

    public List<Equipment> Equipments { get; } = new();
    public List<User> Users { get; } = new();
    public List<Rental> Rentals { get; } = new();

    public int GenerateEquipmentId() => _nextEquipmentId++;
    public int GenerateUserId() => _nextUserId++;
    public int GenerateRentalId() => _nextRentalId++;
}