namespace DreamIsland.Tests.Services
{
    using System.Linq;
    using System.Threading.Tasks;

    using Xunit;

    using DreamIsland.Data.Models;
    using DreamIsland.Services.Partner;
    using DreamIsland.Tests.Mock;

    public class PartnerServiceTests
    {
        [Fact]
        public async Task BecomePartnerShouldAddPartnerInDbAndReturnId()
        {
            const string userId = "TestId";

            using var data = DatabaseMock.Instance;
            var partner = new Partner { UserId = userId, PhoneNumber = "+359888123456", Name = "Test" };

            var partnerService = new PartnerService(data);

            var result = await partnerService.BecomePartner(partner.Name, partner.PhoneNumber, userId);

            Assert.Equal(1, data.Partners.Count());
            Assert.Equal(1, result);
        }

        [Fact]
        public void IsPartnerShouldReturnTrueWhenUserIsPartner()
        {
            const string userId = "TestId";

            using var data = DatabaseMock.Instance;
            data.Partners.Add(new Partner { UserId = userId });
            data.SaveChanges();

            var partnerService = new PartnerService(data);

            var result = partnerService.isPartner(userId);

            Assert.True(result);
        }

        [Fact]
        public void IsPartnerShouldReturnFalseWhenUserIsNotPartner()
        {
            const string userId = "TestId";

            using var data = DatabaseMock.Instance;
            data.Partners.Add(new Partner { UserId = "SomeId" });
            data.SaveChanges();

            var partnerService = new PartnerService(data);

            var result = partnerService.isPartner(userId);

            Assert.False(result);
        }

        [Fact]
        public void IsPartnerShouldReturnCorrectPartnerIdForRegisteredPartner()
        {
            const string userId = "TestId";

            using var data = DatabaseMock.Instance;
            data.Partners.Add(new Partner { Id = 1,UserId = userId });
            data.SaveChanges();

            var partnerService = new PartnerService(data);

            var result = partnerService.PartnerId(userId);

            Assert.Equal(1, result);
        }
    }
}
