using RentalApp.Data;
using RentalApp.Models;
using RentalApp.Services;

namespace RentalApp.UI;

public class ConsoleUI
{
    private readonly RentalDbContext _context;
    private readonly IEquipmentService _equipmentService;
    private readonly IUserService _userService;
    private readonly IRentalService _rentalService;

    public ConsoleUI()
    {
        _context = new RentalDbContext();
        _equipmentService = new EquipmentService(_context);
        _userService = new UserService(_context);
        _rentalService = new RentalService(_context, _equipmentService, _userService);
    }

    public void Run()
    {
        SeedData();

        bool exit = false;
        while (!exit)
        {
            ShowMenu();
            var choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    AddUser();
                    break;
                case "2":
                    AddEquipment();
                    break;
                case "3":
                    ShowAllEquipment();
                    break;
                case "4":
                    ShowAvailableEquipment();
                    break;
                case "5":
                    Rent();
                    break;
                case "6":
                    Return();
                    break;
                case "7":
                    MarkUnavailable();
                    break;
                case "8":
                    ShowUserRentals();
                    break;
                case "9":
                    ShowOverdue();
                    break;
                case "10":
                    Console.WriteLine(_rentalService.GenerateReport());
                    break;
                case "0":
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Nieznana opcja.");
                    break;
            }
        }
    }

    private void ShowMenu()
    {
        Console.WriteLine("\n=== MENU ===");
        Console.WriteLine("1. Dodaj użytkownika");
        Console.WriteLine("2. Dodaj sprzęt");
        Console.WriteLine("3. Wyświetl cały sprzęt");
        Console.WriteLine("4. Wyświetl dostępny sprzęt");
        Console.WriteLine("5. Wypożycz sprzęt");
        Console.WriteLine("6. Zwróć sprzęt");
        Console.WriteLine("7. Oznacz sprzęt jako niedostępny");
        Console.WriteLine("8. Wyświetl aktywne wypożyczenia użytkownika");
        Console.WriteLine("9. Wyświetl przeterminowane wypożyczenia");
        Console.WriteLine("10. Raport podsumowujący");
        Console.WriteLine("0. Wyjście");
        Console.Write("Wybierz: ");
    }

    private void SeedData()
    {
        if (_equipmentService.GetAllEquipment().Any()) return;

        var laptop = new Laptop("Dell XPS 13", "Intel i7", 16, "Windows 11");
        var projector = new Projector("Epson EB-FH06", 3500, "Full HD", true);
        var camera = new Camera("Sony A7 III", 24, "28-70mm", true);

        _equipmentService.AddEquipment(laptop);
        _equipmentService.AddEquipment(projector);
        _equipmentService.AddEquipment(camera);

        var student = new Student("Anna", "Kowalska", "anna@student.com", "s12345");
        var employee = new Employee("Jan", "Nowak", "jan@company.com", "e001", "IT");

        _userService.AddUser(student);
        _userService.AddUser(employee);
    }

    private void AddUser()
    {
        Console.Write("Imię: ");
        var firstName = Console.ReadLine();
        Console.Write("Nazwisko: ");
        var lastName = Console.ReadLine();
        Console.Write("Email: ");
        var email = Console.ReadLine();
        Console.Write("Typ (student/pracownik): ");
        var type = Console.ReadLine()?.ToLower();

        User user;
        if (type == "student")
        {
            Console.Write("Nr indeksu: ");
            var studentId = Console.ReadLine();
            user = new Student(firstName, lastName, email, studentId);
        }
        else
        {
            Console.Write("Nr pracownika: ");
            var empId = Console.ReadLine();
            Console.Write("Departament: ");
            var dept = Console.ReadLine();
            user = new Employee(firstName, lastName, email, empId, dept);
        }

        _userService.AddUser(user);
        Console.WriteLine("Użytkownik dodany.");
    }

    private void AddEquipment()
    {
        Console.Write("Typ (laptop/projektor/kamera): ");
        var type = Console.ReadLine()?.ToLower();
        Console.Write("Nazwa: ");
        var name = Console.ReadLine();

        switch (type)
        {
            case "laptop":
                Console.Write("Procesor: ");
                var cpu = Console.ReadLine();
                Console.Write("RAM (GB): ");
                var ram = int.Parse(Console.ReadLine());
                Console.Write("System operacyjny: ");
                var os = Console.ReadLine();
                var laptop = new Laptop(name, cpu, ram, os);
                _equipmentService.AddEquipment(laptop);
                break;
            case "projektor":
                Console.Write("Lumeny: ");
                var lumens = int.Parse(Console.ReadLine());
                Console.Write("Rozdzielczość: ");
                var res = Console.ReadLine();
                Console.Write("Bezprzewodowy (t/n): ");
                var wireless = Console.ReadLine()?.ToLower() == "t";
                var projector = new Projector(name, lumens, res, wireless);
                _equipmentService.AddEquipment(projector);
                break;
            case "kamera":
                Console.Write("Megapiksele: ");
                var mp = int.Parse(Console.ReadLine());
                Console.Write("Typ obiektywu: ");
                var lens = Console.ReadLine();
                Console.Write("Wideo (t/n): ");
                var video = Console.ReadLine()?.ToLower() == "t";
                var camera = new Camera(name, mp, lens, video);
                _equipmentService.AddEquipment(camera);
                break;
            default:
                Console.WriteLine("Nieznany typ");
                return;
        }
        Console.WriteLine("Sprzęt dodany.");
    }

    private void ShowAllEquipment()
    {
        Console.WriteLine("\nCały sprzęt:");
        foreach (var eq in _equipmentService.GetAllEquipment())
            Console.WriteLine($"  {eq}");
    }

    private void ShowAvailableEquipment()
    {
        Console.WriteLine("\nDostępny sprzęt:");
        foreach (var eq in _equipmentService.GetAvailableEquipment())
            Console.WriteLine($"  {eq}");
    }

    private void Rent()
    {
        ShowAvailableEquipment();
        Console.Write("Podaj ID sprzętu do wypożyczenia: ");
        if (!int.TryParse(Console.ReadLine(), out var eqId)) return;
        Console.Write("Podaj ID użytkownika: ");
        if (!int.TryParse(Console.ReadLine(), out var userId)) return;
        Console.Write("Liczba dni wypożyczenia: ");
        if (!int.TryParse(Console.ReadLine(), out var days)) return;

        try
        {
            var rental = _rentalService.RentEquipment(eqId, userId, days);
            Console.WriteLine($"Wypożyczono. ID wypożyczenia: {rental.Id}, zwrot do {rental.ExpectedReturnDate:yyyy-MM-dd}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Błąd: {ex.Message}");
        }
    }

    private void Return()
    {
        Console.Write("Podaj ID wypożyczenia: ");
        if (!int.TryParse(Console.ReadLine(), out var rentalId)) return;
        Console.Write("Data zwrotu (yyyy-mm-dd) [pusty = dzisiaj]: ");
        var input = Console.ReadLine();
        DateTime returnDate;
        if (string.IsNullOrWhiteSpace(input))
            returnDate = DateTime.Now;
        else if (!DateTime.TryParse(input, out returnDate))
        {
            Console.WriteLine("Niepoprawna data.");
            return;
        }

        var rental = _rentalService.ReturnEquipment(rentalId, returnDate);
        if (rental == null)
            Console.WriteLine("Wypożyczenie nie istnieje lub już zwrócone.");
        else
        {
            Console.WriteLine($"Zwrot zakończony. Kara: {rental.LateFee:C}");
        }
    }

    private void MarkUnavailable()
    {
        ShowAllEquipment();
        Console.Write("Podaj ID sprzętu do oznaczenia jako niedostępny: ");
        if (!int.TryParse(Console.ReadLine(), out var id)) return;
        if (_equipmentService.MarkAsUnavailable(id))
            Console.WriteLine("Sprzęt oznaczony jako niedostępny.");
        else
            Console.WriteLine("Sprzęt nie istnieje.");
    }

    private void ShowUserRentals()
    {
        Console.Write("Podaj ID użytkownika: ");
        if (!int.TryParse(Console.ReadLine(), out var id)) return;
        var rentals = _rentalService.GetActiveRentalsByUser(id);
        if (!rentals.Any())
            Console.WriteLine("Brak aktywnych wypożyczeń.");
        else
        {
            foreach (var r in rentals)
                Console.WriteLine($"Wypożyczenie ID {r.Id}: {r.Equipment.Name}, do {r.ExpectedReturnDate:yyyy-MM-dd}");
        }
    }

    private void ShowOverdue()
    {
        var overdue = _rentalService.GetOverdueRentals(DateTime.Now);
        if (!overdue.Any())
            Console.WriteLine("Brak przeterminowanych wypożyczeń.");
        else
        {
            foreach (var r in overdue)
                Console.WriteLine($"ID {r.Id}: {r.Equipment.Name} -> {r.User.FullName}, oczekiwany zwrot {r.ExpectedReturnDate:yyyy-MM-dd}");
        }
    }
}