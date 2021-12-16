namespace DreamIsland.Services.Partner
{
    using System.Threading.Tasks;
    public interface IPartnerService
    {
        bool isPartner(string userId);
        int PartnerId(string userId);

        Task<int> BecomePartner(string name, string phoneNumber, string userId);
    }
}
