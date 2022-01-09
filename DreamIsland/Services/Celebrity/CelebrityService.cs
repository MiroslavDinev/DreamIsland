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
    using DreamIsland.Services.Celebrity.Models;

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
                PartnerId = partnerId,
                IsPublic = false
            };

            await this.data.Celebrities.AddAsync(celebrity);
            await this.data.SaveChangesAsync();

            return celebrity.Id;
        }

        public AllCelebritiesQueryModel All(string occupation = null, string searchTerm = null, int currentPage = 1)
        {
            var celebritiesQuery = this.data
                .Celebrities
                .Where(c=> c.IsPublic);

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

            var celebrities = this.GetCelebrities(celebritiesQuery
                .OrderByDescending(x => x.Id)
                .Skip((currentPage - 1) * AllCelebritiesQueryModel.ItemsPerPage)
                .Take(AllCelebritiesQueryModel.ItemsPerPage));

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

        public CelebrityDetailsServiceModel Details(int celebrityId)
        {
            var celebrity = this.data.Celebrities
                .Where(x => x.Id == celebrityId)
                .ProjectTo<CelebrityDetailsServiceModel>(this.mapper.ConfigurationProvider)
                .FirstOrDefault();

            return celebrity;
        }

        public async Task<bool> EditAsync(int celebrityId, string name, string occupation, string description,
            string imageUrl, int? age, bool isPublic)
        {
            var celebrity = this.data.Celebrities.Find(celebrityId);

            if(celebrity == null)
            {
                return false;
            }

            celebrity.Name = name;
            celebrity.Occupation = occupation;
            celebrity.Description = description;
            celebrity.ImageUrl = imageUrl;
            celebrity.Age = age;
            celebrity.IsPublic = isPublic;

            await this.data.SaveChangesAsync();

            return true;
        }

        public IEnumerable<CelebrityListingViewModel> GetCelebritiesByPartner(string userId)
        {
            var celebrities = this.GetCelebrities(this.data
                .Celebrities
                .Where(x => x.Partner.UserId == userId)
                .OrderByDescending(x=> x.Id));

            return celebrities;
        }

        public bool IsByPartner(int celebrityId, int partnerId)
        {
            var isByPartner = this.data
                .Celebrities
                .Any(x => x.Id == celebrityId && x.PartnerId == partnerId);

            return isByPartner;
        }

        private IEnumerable<CelebrityListingViewModel> GetCelebrities(IQueryable celebritiesQuery)
        {
            var celebrities = celebritiesQuery
                .ProjectTo<CelebrityListingViewModel>(this.mapper.ConfigurationProvider)
                .ToList();

            return celebrities;
        }
    }
}
