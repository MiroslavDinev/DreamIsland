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
    using DreamIsland.Services.Car.Models;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<Island, IslandListingViewModel>();
            this.CreateMap<Island, IslandDetailsServiceModel>()
                .ForMember(x => x.UserId, cfg => cfg.MapFrom(x => x.Partner.UserId));
            this.CreateMap<IslandDetailsServiceModel, IslandFormModel>();
            this.CreateMap<IslandRegion, IslandRegionServiceModel>();
            this.CreateMap<PopulationSize, IslandPopulationSizeServiceModel>();

            this.CreateMap<Car, CarListingViewModel>();
            this.CreateMap<Car, CarDetailsServiceModel>()
                .ForMember(x => x.UserId, cfg => cfg.MapFrom(x => x.Partner.UserId));
            this.CreateMap<CarDetailsServiceModel, CarFormModel>();

            this.CreateMap<Celebrity, CelebrityListingViewModel>();
        }
    }
}

