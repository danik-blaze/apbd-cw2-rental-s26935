namespace RentalApp.Models;

public class Camera : Equipment
{
    public int Megapixels { get; set; }
    public string LensType { get; set; } = string.Empty;
    public bool HasVideo { get; set; }

    public Camera(string name, int megapixels, string lens, bool video) : base(name)
    {
        Megapixels = megapixels;
        LensType = lens;
        HasVideo = video;
    }
}
