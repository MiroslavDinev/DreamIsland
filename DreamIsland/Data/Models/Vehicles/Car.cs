namespace DreamIsland.Data.Models.Vehicles
{
    using DreamIsland.Data.Models.Islands;
    public class Car : Vehicle
    {
        public bool HasRemoteStart { get; set; }

        public bool HasRemoteControlParking { get; set; }

        public bool HasSeatMassager { get; set; }

        public int PartnerId { get; set; }

        public Partner Partner { get; set; }

        public int? IslandId { get; set; }

        public Island Island { get; set; }
    }
}
