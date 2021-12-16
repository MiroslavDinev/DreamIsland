namespace DreamIsland.Services.Partner
{
    using System.Linq;
    using System.Threading.Tasks;

    using DreamIsland.Data;
    using Data.Models;

    public class PartnerService : IPartnerService
    {
        private readonly DreamIslandDbContext data;

        public PartnerService(DreamIslandDbContext data)
        {
            this.data = data;
        }

        public async Task<int> BecomePartner(string name, string phoneNumber, string userId)
        {
            var partner = new Partner
            {
                Name = name,
                PhoneNumber = phoneNumber,
                UserId = userId
            };

            await this.data.Partners.AddAsync(partner);
            await this.data.SaveChangesAsync();

            return partner.Id;
        }

        public bool isPartner(string userId)
        {
            return this.data.Partners
                .Any(x => x.UserId == userId);
        }

        public int PartnerId(string userId)
        {
             var partnerId = this.data.Partners
                .Where(p => p.UserId == userId)
                .Select(p => p.Id)
                .FirstOrDefault();

            return partnerId;
        }
    }
}
