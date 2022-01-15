﻿namespace DreamIsland.Services.Celebrity.Models
{
    using System.Collections.Generic;

    using DreamIsland.Models;

    public class CelebrityDetailsServiceModel : BaseServiceModel
    {
        public string Name { get; set; }
        public string Occupation { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public int? Age { get; set; }
        public List<GalleryModel> Gallery { get; set; }
    }
}
