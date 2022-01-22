namespace DreamIsland.Infrastructure
{
    using AutoMapper;

    using DreamIsland.Data.Models;
    using DreamIsland.Data.Models.Islands;
    using DreamIsland.Data.Models.Vehicles;
    using DreamIsland.Data.Models.Celebrities;
    using DreamIsland.Models.Cars;
    using DreamIsland.Models.Islands;
    using DreamIsland.Models.Celebrities;
    using DreamIsland.Services.Island.Models;
    using DreamIsland.Services.Car.Models;
    using DreamIsland.Services.Celebrity.Models;
    using DreamIsland.Services.Collectible.Models;
    using DreamIsland.Models.Collectibles;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<Island, IslandListingViewModel>();
            this.CreateMap<Island, LatestIslandsServiceModel>();
            this.CreateMap<Island, IslandDetailsServiceModel>()
                .ForMember(x => x.UserId, cfg => cfg.MapFrom(x => x.Partner.UserId));

            this.CreateMap<IslandDetailsServiceModel, IslandEditFormModel>();
            this.CreateMap<IslandRegion, IslandRegionServiceModel>();
            this.CreateMap<PopulationSize, IslandPopulationSizeServiceModel>();

            this.CreateMap<Car, CarListingViewModel>();
            this.CreateMap<Car, CarDetailsServiceModel>()
                .ForMember(x => x.UserId, cfg => cfg.MapFrom(x => x.Partner.UserId));

            this.CreateMap<CarDetailsServiceModel, CarEditFormModel>();

            this.CreateMap<Celebrity, CelebrityListingViewModel>();
            this.CreateMap<Celebrity, CelebrityDetailsServiceModel>()
                .ForMember(x => x.UserId, cfg => cfg.MapFrom(x => x.Partner.UserId));

            this.CreateMap<CelebrityDetailsServiceModel, CelebrityEditFormModel>();

            this.CreateMap<Collectible, CollectibleDetailsServiceModel>()
                .ForMember(x => x.UserId, cfg => cfg.MapFrom(x => x.Partner.UserId));

            this.CreateMap<CollectibleDetailsServiceModel, CollectibleFormModel>();
        }
    }
}

