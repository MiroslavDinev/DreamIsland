namespace DreamIsland.Tests.Data
{
    using System.Linq;
    using System.Collections.Generic;

    using DreamIsland.Data.Models.Islands;

    public static class Islands
    {
        public static IEnumerable<Island> TenPublicIslands()
        {
            return Enumerable.Range(0, 10).Select(i => new Island
            {
                IsPublic = true
            });
        }
    }
}
