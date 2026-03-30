namespace RentalApp.Models;

public abstract class User
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public abstract int MaxActiveRentals { get; }

    protected User() { }

    protected User(string firstName, string lastName, string email)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }

    public string FullName => $"{FirstName} {LastName}";

    public override string ToString()
    {
        return $"{GetType().Name}: {FullName} (ID: {Id})";
    }
}
