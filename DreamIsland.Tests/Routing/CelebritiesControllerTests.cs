namespace DreamIsland.Tests.Routing
{
    using Xunit;
    using MyTested.AspNetCore.Mvc;

    using DreamIsland.Controllers;
    using DreamIsland.Models.Celebrities;

    public class CelebritiesControllerTests
    {
        [Fact]
        public void AllRouteShouldBeMapped()
        {
            MyRouting
                .Configuration()
                .ShouldMap("/Celebrities/All")
                .To<CelebritiesController>(c => c.All(new AllCelebritiesQueryModel()));
        }

        [Fact]
        public void MyRouteShouldBeMapped()
        {
            MyRouting
                .Configuration()
                .ShouldMap("/Celebrities/My")
                .To<CelebritiesController>(c => c.My());
        }

        [Fact]
        public void GetAddRouteShouldBeMapped()
        {
            MyRouting
                .Configuration()
                .ShouldMap("/Celebrities/Add")
                .To<CelebritiesController>(c => c.Add());
        }

        [Fact]
        public void PostAddRouteShouldBeMapped()
        {
            MyRouting
                .Configuration()
                .ShouldMap(request => request
                .WithPath("/Celebrities/Add")
                .WithMethod(HttpMethod.Post))
                .To<CelebritiesController>(c => c.Add(With.Any<CelebrityAddFormModel>()));

        }

        [Fact]
        public void GetEditRouteShouldBeMapped()
        {
            MyRouting
                .Configuration()
                .ShouldMap("/Celebrities/Edit/1")
                .To<CelebritiesController>(c => c.Edit(1));
        }

        [Fact]
        public void PostEditRouteShouldBeMapped()
        {
            MyRouting
                .Configuration()
                .ShouldMap(request => request
                .WithPath("/Celebrities/Edit")
                .WithMethod(HttpMethod.Post))
                .To<CelebritiesController>(c => c.Edit(With.Any<CelebrityEditFormModel>()));

        }

        [Fact]
        public void DeleteRouteShouldBeMapped()
        {
            MyRouting
                .Configuration()
                .ShouldMap("/Celebrities/Delete/1")
                .To<CelebritiesController>(c => c.Delete(1));
        }

        [Fact]
        public void DetailsRouteShouldBeMapped()
        {
            MyRouting
                .Configuration()
                .ShouldMap("/Celebrities/Details/1/TheRock-Wrestler")
                .To<CelebritiesController>(c => c.Details(1, "TheRock-Wrestler"));
        }
    }
}
