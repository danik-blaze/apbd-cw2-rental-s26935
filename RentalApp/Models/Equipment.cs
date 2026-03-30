namespace RentalApp.Models;

public abstract class Equipment
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsAvailable { get; set; } = true;
    public string Status { get; set; } = "available";

    protected Equipment() { }

    protected Equipment(string name)
    {
        Name = name;
    }

    public override string ToString()
    {
        return $"{GetType().Name}: {Name} (ID: {Id}) - {(IsAvailable ? "dostępny" : "niedostępny")}";
    }
}