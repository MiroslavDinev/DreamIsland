namespace DreamIsland.Data.Models.Vehicles
{
    public class Car : Vehicle
    {
        public bool HasRemoteStart { get; set; }

        public bool HasRemoteControlParking { get; set; }

        public bool HasSeatMassager { get; set; }

        public int? IslandId { get; set; }

        public Island Island { get; set; }
    }
}
