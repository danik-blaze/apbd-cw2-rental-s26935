using RentalApp.Models;

namespace RentalApp.Services;

public interface IEquipmentService
{
    Equipment AddEquipment(Equipment equipment);
    Equipment? FindById(int id);
    List<Equipment> GetAllEquipment();
    List<Equipment> GetAvailableEquipment();
    bool MarkAsUnavailable(int id);
}