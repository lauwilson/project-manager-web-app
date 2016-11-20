using Hemlock;
using System.Web.Routing;
using NUnit.Framework;
using HemlockTests.Mocks;
using System.Web.Mvc;
using Mvc5RouteUnitTester;

namespace HemlockTests
{
    [TestFixture]
    class UserControllerTest
    {
        RouteTester<MvcApplication> routeTester;

        [SetUp]
        public void Initialize()
        {
            // Assemble
            routeTester = new RouteTester<MvcApplication>(typeof(RouteConfig));
        }

        [Test]
        public void VerifyUser_ShouldReturnCorrectInboundRoute()
        {
            // Assert
            routeTester.WithIncomingRequest("/User/VerifyUser").ShouldMatchRoute("User", "VerifyUser");
        }

        [Test]
        public void Login_ShouldReturnCorrectInboundRoute()
        {
            // Assert
            routeTester.WithIncomingRequest("/User/Login").ShouldMatchRoute("User", "Login");
        }

        [Test]
        public void Logout_ShouldReturnCorrectInboundRoute()
        {
            // Assert
            routeTester.WithIncomingRequest("/User/Logout").ShouldMatchRoute("User", "Logout");
        }

    }
}
