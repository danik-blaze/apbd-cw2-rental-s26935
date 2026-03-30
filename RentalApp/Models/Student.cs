namespace RentalApp.Models;

public class Student : User
{
    public string StudentId { get; set; } = string.Empty;

    public override int MaxActiveRentals => 2;

    public Student(string firstName, string lastName, string email, string studentId)
        : base(firstName, lastName, email)
    {
        StudentId = studentId;
    }
}
