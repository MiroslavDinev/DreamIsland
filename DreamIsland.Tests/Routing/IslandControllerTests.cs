namespace DreamIsland.Tests.Routing
{
    using Xunit;
    using MyTested.AspNetCore.Mvc;

    using DreamIsland.Controllers;
    using DreamIsland.Models.Islands;

    public class IslandControllerTests
    {
        [Fact]
        public void AllRouteShouldBeMapped()
        {
            MyRouting
                .Configuration()
                .ShouldMap("/Islands/All")
                .To<IslandsController>(c => c.All(new AllIslandsQueryModel()));
        }

        [Fact]
        public void MyRouteShouldBeMapped()
        {
            MyRouting
                .Configuration()
                .ShouldMap("/Islands/My")
                .To<IslandsController>(c => c.My());
        }

        [Fact]
        public void GetAddRouteShouldBeMapped()
        {
            MyRouting
                .Configuration()
                .ShouldMap("/Islands/Add")
                .To<IslandsController>(c => c.Add());
        }

        [Fact]
        public void PostAddRouteShouldBeMapped()
        {
            MyRouting
                .Configuration()
                .ShouldMap(request => request
                .WithPath("/Islands/Add")
                .WithMethod(HttpMethod.Post))
                .To<IslandsController>(c => c.Add(With.Any<IslandAddFormModel>()));

        }

        [Fact]
        public void GetEditRouteShouldBeMapped()
        {
            MyRouting
                .Configuration()
                .ShouldMap("/Islands/Edit/1")
                .To<IslandsController>(c => c.Edit(1));
        }

        [Fact]
        public void PostEditRouteShouldBeMapped()
        {
            MyRouting
                .Configuration()
                .ShouldMap(request => request
                .WithPath("/Islands/Edit")
                .WithMethod(HttpMethod.Post))
                .To<IslandsController>(c => c.Edit(With.Any<IslandEditFormModel>()));

        }

        [Fact]
        public void DeleteRouteShouldBeMapped()
        {
            MyRouting
                .Configuration()
                .ShouldMap("/Islands/Delete/1")
                .To<IslandsController>(c => c.Delete(1));
        }

        [Fact]
        public void DetailsRouteShouldBeMapped()
        {
            MyRouting
                .Configuration()
                .ShouldMap("/Islands/Details/1/UK")
                .To<IslandsController>(c => c.Details(1, "UK"));
        }
    }
}
