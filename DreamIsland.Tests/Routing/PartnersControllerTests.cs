namespace DreamIsland.Tests.Routing
{
    using Xunit;
    using MyTested.AspNetCore.Mvc;

    using DreamIsland.Controllers;
    using DreamIsland.Models.Partners;

    public class PartnersControllerTests
    {
        [Fact]
        public void GetBecomeShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Partners/Become")
                .To<PartnersController>(c => c.Become());

        [Fact]
        public void PostBecomeShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Partners/Become")
                    .WithMethod(HttpMethod.Post))
                .To<PartnersController>(c => c.Become(With.Any<BecomePartnerFormModel>()));
    }
}
