﻿namespace DreamIsland.Services.Car
{
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using DreamIsland.Models.Cars;
    using DreamIsland.Models.Cars.Enums;
    using DreamIsland.Services.Car.Models;

    public interface ICarService
    {
        Task<int> AddAsync(string brand, string model, string description, string imageUrl, int year,
            bool hasRemoteStart, bool hasRemoteControlParking, bool hasSeatMassager, int partnerId);

        Task<bool> EditAsync(int carId, string brand, string model, string description, string imageUrl, int year,
            bool hasRemoteStart, bool hasRemoteControlParking, bool hasSeatMassager, bool isPublic);

        AllCarsQueryModel All(string brand = null, string searchTerm = null, CarsSorting carSorting = CarsSorting.DateAdded, int currentPage = 1);

        IEnumerable<CarListingViewModel> GetCarsByPartner(string userId);

        CarDetailsServiceModel Details(int carId);

        bool IsByPartner(int carId, int partnerId);
    }
}
