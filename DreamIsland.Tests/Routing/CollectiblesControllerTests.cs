namespace DreamIsland.Tests.Routing
{
    using Xunit;
    using MyTested.AspNetCore.Mvc;

    using DreamIsland.Controllers;
    using DreamIsland.Models.Collectibles;

    public class CollectiblesControllerTests
    {
        [Fact]
        public void AllRouteShouldBeMapped()
        {
            MyRouting
                .Configuration()
                .ShouldMap("/Collectibles/All")
                .To<CollectiblesController>(c => c.All(new AllCollectiblesQueryModel()));
        }

        [Fact]
        public void MyRouteShouldBeMapped()
        {
            MyRouting
                .Configuration()
                .ShouldMap("/Collectibles/My")
                .To<CollectiblesController>(c => c.My());
        }

        [Fact]
        public void GetAddRouteShouldBeMapped()
        {
            MyRouting
                .Configuration()
                .ShouldMap("/Collectibles/Add")
                .To<CollectiblesController>(c => c.Add());
        }

        [Fact]
        public void PostAddRouteShouldBeMapped()
        {
            MyRouting
                .Configuration()
                .ShouldMap(request => request
                .WithPath("/Collectibles/Add")
                .WithMethod(HttpMethod.Post))
                .To<CollectiblesController>(c => c.Add(With.Any<CollectibleAddFormModel>()));

        }

        [Fact]
        public void GetEditRouteShouldBeMapped()
        {
            MyRouting
                .Configuration()
                .ShouldMap("/Collectibles/Edit/1")
                .To<CollectiblesController>(c => c.Edit(1));
        }

        [Fact]
        public void PostEditRouteShouldBeMapped()
        {
            MyRouting
                .Configuration()
                .ShouldMap(request => request
                .WithPath("/Collectibles/Edit")
                .WithMethod(HttpMethod.Post))
                .To<CollectiblesController>(c => c.Edit(With.Any<CollectibleEditFormModel>()));

        }

        [Fact]
        public void DeleteRouteShouldBeMapped()
        {
            MyRouting
                .Configuration()
                .ShouldMap("/Collectibles/Delete/1")
                .To<CollectiblesController>(c => c.Delete(1));
        }

        [Fact]
        public void DetailsRouteShouldBeMapped()
        {
            MyRouting
                .Configuration()
                .ShouldMap("/Collectibles/Details/1/Sword")
                .To<CollectiblesController>(c => c.Details(1, "Sword"));
        }
    }
}
