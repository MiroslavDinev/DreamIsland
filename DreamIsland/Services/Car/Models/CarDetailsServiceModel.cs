namespace DreamIsland.Services.Car.Models
{
    public class CarDetailsServiceModel : BaseServiceModel
    {
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public int Year { get; set; }
        public bool HasRemoteStart { get; set; }
        public bool HasRemoteControlParking { get; set; }
        public bool HasSeatMassager { get; set; }
    }
}
