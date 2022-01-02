namespace DreamIsland.Services.Celebrity
{
    using System.Linq;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using Data.Models;
    using DreamIsland.Data;
    using DreamIsland.Models.Celebrities;
    

    public class CelebrityService : ICelebrityService
    {
        private readonly DreamIslandDbContext data;
        private readonly IMapper mapper;

        public CelebrityService(DreamIslandDbContext data, IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper;
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

        public AllCelebritiesQueryModel All(string occupation = null, string searchTerm = null, int currentPage = 1)
        {
            var celebritiesQuery = this.data
                .Celebrities
                .AsQueryable();

            if (!string.IsNullOrEmpty(occupation))
            {
                celebritiesQuery = celebritiesQuery
                    .Where(x => x.Occupation == occupation);
            }

            if(!string.IsNullOrEmpty(searchTerm))
            {
                celebritiesQuery = celebritiesQuery
                    .Where(x => x.Name.ToLower().Contains(searchTerm.ToLower()));
            }

            var totalCelebrities = celebritiesQuery.Count();

            var celebrities = celebritiesQuery
                .OrderByDescending(x=> x.Id)
                .Skip((currentPage - 1) * AllCelebritiesQueryModel.ItemsPerPage)
                .Take(AllCelebritiesQueryModel.ItemsPerPage)
                .ProjectTo<CelebrityListingViewModel>(this.mapper.ConfigurationProvider)
                .ToList();

            var occupations = this.data
                .Celebrities
                .Select(x => x.Occupation)
                .Distinct()
                .OrderBy(x => x)
                .ToList();

            var celebrity = new AllCelebritiesQueryModel
            {
                Celebrities = celebrities,
                Occupations = occupations,
                TotalItems = totalCelebrities,
                CurrentPage = currentPage,
                Occupation = occupation,
                SearchTerm = searchTerm
            };

            return celebrity;
        }
    }
}
