namespace RentalApp.Models;

public class Rental
{
    public int Id { get; set; }
    public Equipment Equipment { get; set; }
    public User User { get; set; }
    public DateTime RentalDate { get; set; }
    public DateTime ExpectedReturnDate { get; set; }
    public DateTime? ActualReturnDate { get; set; }
    public decimal LateFee { get; set; }

    public bool IsActive => ActualReturnDate == null;

    public Rental(Equipment equipment, User user, DateTime rentalDate, int rentalDays)
    {
        Equipment = equipment;
        User = user;
        RentalDate = rentalDate;
        ExpectedReturnDate = rentalDate.AddDays(rentalDays);
    }

    public bool IsOverdue(DateTime currentDate)
    {
        return IsActive && currentDate > ExpectedReturnDate;
    }

    public decimal CalculateLateFee(DateTime returnDate, decimal feePerDay)
    {
        if (!IsActive || returnDate <= ExpectedReturnDate) return 0;
        int daysOverdue = (returnDate - ExpectedReturnDate).Days;
        return daysOverdue * feePerDay;
    }
}
