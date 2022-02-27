namespace DreamIsland.Tests.Controllers
{
    using Xunit;
    using MyTested.AspNetCore.Mvc;

    using DreamIsland.Controllers;

    public class CarsControllerTests
    {
        [Fact]
        public void DetailsShouldReturnNotFoundWhenInvalidCarId()
        {
            MyController<CarsController>
                .Calling(c => c.Details(int.MaxValue, string.Empty))
                .ShouldReturn()
                .NotFound();
        }
    }
}
