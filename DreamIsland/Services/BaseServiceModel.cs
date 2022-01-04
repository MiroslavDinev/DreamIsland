namespace DreamIsland.Services
{
    public abstract class BaseServiceModel
    {
        public int Id { get; set; }
        public int PartnerId { get; set; }
        public string PartnerName { get; set; }
        public string UserId { get; set; }
    }
}
