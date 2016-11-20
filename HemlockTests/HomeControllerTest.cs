using Hemlock;
using System.Web.Routing;
using NUnit.Framework;
using HemlockTests.Mocks;

namespace HemlockTests
{
    class HomeControllerTest
    {
        [Test]
        public void Index_ShouldReturnCorrectInboundRoute()
        {
            // Assemble
            RouteCollection _routes = new RouteCollection();
            RouteConfig.RegisterRoutes(_routes);

            MockHttpContext mockHttpContext = new MockHttpContext();
            mockHttpContext.Request.Setup(x => x.AppRelativeCurrentExecutionFilePath)
                                   .Returns("~/");

            // Act
            RouteData sut = _routes.GetRouteData(mockHttpContext.HttpContextBase.Object);

            // Assert
            Assert.IsNotNull(sut, "Did not find the route");
            Assert.AreEqual("Home", sut.Values["controller"]);
            Assert.AreEqual("Index", sut.Values["action"]);
        }
    }
}
