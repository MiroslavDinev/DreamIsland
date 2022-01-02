namespace DreamIsland.Infrastructure
{
    using AutoMapper;

    using DreamIsland.Data.Models;
    using DreamIsland.Data.Models.Islands;
    using DreamIsland.Data.Models.Vehicles;
    using DreamIsland.Models.Cars;
    using DreamIsland.Models.Islands;
    using DreamIsland.Models.Celebrities;
    using DreamIsland.Services.Island.Models;


    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<Island, IslandListingViewModel>();
            this.CreateMap<IslandRegion, IslandRegionServiceModel>();
            this.CreateMap<PopulationSize, IslandPopulationSizeServiceModel>();
            this.CreateMap<Car, CarListingViewModel>();
            this.CreateMap<Celebrity, CelebrityListingViewModel>();
        }
    }
}

