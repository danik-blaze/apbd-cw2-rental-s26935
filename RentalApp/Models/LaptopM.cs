namespace RentalApp.Models;

public class LaptopM : Equipment
{
    public string Processor { get; set; } = string.Empty;
    public int RamGB { get; set; }
    public string OperatingSystem { get; set; } = string.Empty;

    public LaptopM(string name, string processor, int ramGB, string os) : base(name)
    {
        Processor = processor;
        RamGB = ramGB;
        OperatingSystem = os;
    }
}
