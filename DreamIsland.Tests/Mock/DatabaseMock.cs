namespace DreamIsland.Tests.Mock
{
    using System;

    using Microsoft.EntityFrameworkCore;

    using DreamIsland.Data;

    public static class DatabaseMock
    {
        public static DreamIslandDbContext Instance
        {
            get
            {
                var dbContextOptions = new DbContextOptionsBuilder<DreamIslandDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

                return new DreamIslandDbContext(dbContextOptions);
            }
        }
    }
}
