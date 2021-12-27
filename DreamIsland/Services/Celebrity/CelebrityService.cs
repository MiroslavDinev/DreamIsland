namespace DreamIsland.Services.Celebrity
{
    using System.Linq;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using Data.Models;
    using DreamIsland.Data;
    using DreamIsland.Models.Celebrities;

    public class CelebrityService : ICelebrityService
    {
        private readonly DreamIslandDbContext data;

        public CelebrityService(DreamIslandDbContext data)
        {
            this.data = data;
        }
        public async Task<int> AddAsync(string name, string occupation, string description, string imageUrl, int? age, int partnerId)
        {
            var celebrity = new Celebrity
            {
                Name = name,
                Occupation = occupation,
                Description = description,
                ImageUrl = imageUrl,
                Age = age,
                PartnerId = partnerId
            };

            await this.data.Celebrities.AddAsync(celebrity);
            await this.data.SaveChangesAsync();

            return celebrity.Id;
        }

        public IEnumerable<CelebrityListingViewModel> All()
        {
            var celebrities = this.data
                .Celebrities
                .OrderByDescending(x=> x.Id)
                .Select(x => new CelebrityListingViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Occupation = x.Occupation,
                    ImageUrl = x.ImageUrl
                })
                .ToList();

            return celebrities;
        }
    }
}
