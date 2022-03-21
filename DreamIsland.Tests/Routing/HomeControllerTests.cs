namespace DreamIsland.Tests.Routing
{
    using Xunit;
    using MyTested.AspNetCore.Mvc;

    using DreamIsland.Controllers;

    public class HomeControllerTests
    {
        [Fact]
        public void IndexRouteShouldBeMapped()
        {
            MyRouting
                .Configuration()
                .ShouldMap("/")
                .To<HomeController>(c => c.Index());
        }

        [Fact]
        public void ChatRouteShouldBeMapped()
        {
            MyRouting
                .Configuration()
                .ShouldMap("/Home/Chat")
                .To<HomeController>(c => c.Chat());
        }

        [Fact]
        public void PrivacyRouteShouldBeMapped()
        {
            MyRouting
                .Configuration()
                .ShouldMap("/Home/Privacy")
                .To<HomeController>(c => c.Privacy());
        }

    }
}
