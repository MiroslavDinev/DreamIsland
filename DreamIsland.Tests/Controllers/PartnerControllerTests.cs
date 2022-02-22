namespace DreamIsland.Tests.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;
    using MyTested.AspNetCore.Mvc;
    using DreamIsland.Controllers;
    using DreamIsland.Models.Partners;
    using DreamIsland.Data.Models;
    using static WebConstants.GlobalMessages;

    public class PartnerControllerTests
    {
        [Fact]
        public void GetBecomeIsForAuthorizedUsersAndReturnsView()
        {
            MyController<PartnersController>
                .Instance()
                .Calling(c => c.Become())
                .ShouldHave()
                .ActionAttributes(attributes => attributes.
                RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .View();
        }

        [Theory]
        [InlineData("Partner", "+359888123456")]
        public void PostBecomeShouldBeForAuthorizedUsersAndReturnRedirect(string partnerName, string phoneNumber)
        {
            MyController<PartnersController>
                .Instance(controller => controller
                .WithUser())
                .Calling(c => c.Become(new BecomePartnerFormModel
                {
                    Name = partnerName,
                    PhoneNumber = phoneNumber
                }))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                .RestrictingForHttpMethod(HttpMethod.Post)
                .RestrictingForAuthorizedRequests())
                .ValidModelState()
                .Data(data => data
                .WithSet<Partner>(partners => partners
                .Any(p =>
                p.Name == partnerName &&
                p.PhoneNumber == phoneNumber &&
                p.UserId == TestUser.Identifier)))
                .TempData(tempData => tempData
                .ContainingEntryWithKey(SuccessMessageKey))
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                .To<HomeController>(c => c.Index()));
        }

        [Fact]
        public void PostBecomeShouldReturnViewWithSameModelWhenStateIsInvalid()
        {
            MyController<PartnersController>
                .Instance(controller => controller
                .WithUser())
                .Calling(c => c.Become(With.Default<BecomePartnerFormModel>()))
                .ShouldHave()
                .InvalidModelState()
                .AndAlso()
                .ShouldReturn()
                .View(With.Default<BecomePartnerFormModel>());
        }
    }
}
