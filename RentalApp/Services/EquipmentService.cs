using RentalApp.Data;
using RentalApp.Models;

namespace RentalApp.Services;

public class EquipmentService : IEquipmentService
{
    private readonly RentalDbContext _context;

    public EquipmentService(RentalDbContext context)
    {
        _context = context;
    }

    public Equipment AddEquipment(Equipment equipment)
    {
        equipment.Id = _context.GenerateEquipmentId();
        _context.Equipments.Add(equipment);
        return equipment;
    }

    public Equipment? FindById(int id) => _context.Equipments.FirstOrDefault(e => e.Id == id);

    public List<Equipment> GetAllEquipment() => _context.Equipments.ToList();

    public List<Equipment> GetAvailableEquipment() => _context.Equipments.Where(e => e.IsAvailable).ToList();

    public bool MarkAsUnavailable(int id)
    {
        var eq = FindById(id);
        if (eq == null) return false;
        eq.IsAvailable = false;
        eq.Status = "unavailable";
        return true;
    }
}