namespace DreamIsland.Data
{
    public static class DataConstants
    {
        public static class Vehicle
        {
            public const int BrandMaxLength = 30;
            public const int BrandMinLength = 2;
            public const int ModelMaxLength = 30;
            public const int ModelMinLength = 2;
            public const int DescriptionMaxLength = 3000;
            public const int DescriptionMinLength = 10;
            public const int MinYear = 1900;
            public const int MaxYear = 2030;
        }

        public static class Island
        {
            public const int NameMaxLength = 30;
            public const int LocationMaxLength = 50;
        }
    }
}
