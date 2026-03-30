namespace RentalApp.Models;

public class Employee : User
{
    public string EmployeeId { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;

    public override int MaxActiveRentals => 5;

    public Employee(string firstName, string lastName, string email, string employeeId, string department)
        : base(firstName, lastName, email)
    {
        EmployeeId = employeeId;
        Department = department;
    }
}