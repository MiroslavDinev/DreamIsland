namespace DreamIsland.Tests.Routing
{
    using Xunit;
    using MyTested.AspNetCore.Mvc;

    using DreamIsland.Controllers;
    using DreamIsland.Models;

    public class ContactControllerTests
    {
        [Fact]
        public void GetBookRouteShouldBeMapped()
        {
            MyRouting
                .Configuration()
                .ShouldMap("/Contact/Book")
                .To<ContactController>(c => c.Book());
        }

        [Fact]
        public void PostBookRouteShouldBeMapped()
        {
            MyRouting
                .Configuration()
                .ShouldMap(request => request
                .WithPath("/Contact/Book")
                .WithMethod(HttpMethod.Post))
                .To<ContactController>(c => c.Book(With.Any<ContactFormViewModel>()));

        }
    }
}
