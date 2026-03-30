namespace RentalApp.Models;

public class Projector : Equipment
{
    public int Lumens { get; set; }
    public string Resolution { get; set; } = string.Empty;
    public bool HasWireless { get; set; }

    public Projector(string name, int lumens, string resolution, bool wireless) : base(name)
    {
        Lumens = lumens;
        Resolution = resolution;
        HasWireless = wireless;
    }
}