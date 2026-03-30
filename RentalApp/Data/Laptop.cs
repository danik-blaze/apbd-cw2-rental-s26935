namespace RentalApp.Models;

public class Laptop : Equipment
{
    public string Processor { get; set; } = string.Empty;
    public int RamGB { get; set; }
    public string OperatingSystem { get; set; } = string.Empty;

    public Laptop(string name, string processor, int ramGB, string os) : base(name)
    {
        Processor = processor;
        RamGB = ramGB;
        OperatingSystem = os;
    }
}