using Hemlock;
using System.Web.Routing;
using NUnit.Framework;
using HemlockTests.Mocks;
using System.Web.Mvc;
using Mvc5RouteUnitTester;

namespace HemlockTests
{
    [TestFixture]
    class ErrorControllerTest
    {
        RouteCollection _routes;

        [SetUp]
        public void Initialize()
        {
            _routes = new RouteCollection();
            RouteConfig.RegisterRoutes(_routes);
        }

        [Test]
        public void BadRequest_ShouldReturnCorrectInboundRoute()
        {
            // Assemble
            MockHttpContext mockHttpContext = new MockHttpContext();
            mockHttpContext.Request.Setup(x => x.AppRelativeCurrentExecutionFilePath)
                                   .Returns("~/Error/BadRequest");

            // Act
            RouteData sut = _routes.GetRouteData(mockHttpContext.HttpContextBase.Object);

            // Assert
            Assert.IsNotNull(sut, "Did not find the route");
            Assert.AreEqual("Error", sut.Values["controller"]);
            Assert.AreEqual("BadRequest", sut.Values["action"]);
        }

        [Test]
        public void LoginRequired_ShouldReturnCorrectInboundRoute()
        {
            // Assemble
            MockHttpContext mockHttpContext = new MockHttpContext();
            mockHttpContext.Request.Setup(x => x.AppRelativeCurrentExecutionFilePath)
                                   .Returns("~/Error/LoginRequired");

            // Act
            RouteData sut = _routes.GetRouteData(mockHttpContext.HttpContextBase.Object);

            // Assert
            Assert.IsNotNull(sut, "Did not find the route");
            Assert.AreEqual("Error", sut.Values["controller"]);
            Assert.AreEqual("LoginRequired", sut.Values["action"]);
        }

        [Test]
        public void NotFound_ShouldReturnCorrectInboundRoute()
        {
            // Assemble
            MockHttpContext mockHttpContext = new MockHttpContext();
            mockHttpContext.Request.Setup(x => x.AppRelativeCurrentExecutionFilePath)
                                   .Returns("~/Error/NotFound");

            // Act
            RouteData sut = _routes.GetRouteData(mockHttpContext.HttpContextBase.Object);

            // Assert
            Assert.IsNotNull(sut, "Did not find the route");
            Assert.AreEqual("Error", sut.Values["controller"]);
            Assert.AreEqual("NotFound", sut.Values["action"]);
        }

        [Test]
        public void PermissionDenied_ShouldReturnCorrectInboundRoute()
        {
            // Assemble
            MockHttpContext mockHttpContext = new MockHttpContext();
            mockHttpContext.Request.Setup(x => x.AppRelativeCurrentExecutionFilePath)
                                   .Returns("~/Error/PermissionDenied");

            // Act
            RouteData sut = _routes.GetRouteData(mockHttpContext.HttpContextBase.Object);

            // Assert
            Assert.IsNotNull(sut, "Did not find the route");
            Assert.AreEqual("Error", sut.Values["controller"]);
            Assert.AreEqual("PermissionDenied", sut.Values["action"]);
        }

        [Test]
        public void ServerError_ShouldReturnCorrectInboundRoute()
        {
            // Assemble
            MockHttpContext mockHttpContext = new MockHttpContext();
            mockHttpContext.Request.Setup(x => x.AppRelativeCurrentExecutionFilePath)
                                   .Returns("~/Error/ServerError");

            // Act
            RouteData sut = _routes.GetRouteData(mockHttpContext.HttpContextBase.Object);

            // Assert
            Assert.IsNotNull(sut, "Did not find the route");
            Assert.AreEqual("Error", sut.Values["controller"]);
            Assert.AreEqual("ServerError", sut.Values["action"]);
        }

        [Test]
        public void BadRequest_ShouldReturnCorrectOutboundRoute()
        {
            // Assemble
            var sut = new RouteTester<MvcApplication>(typeof(RouteConfig));
            // Act and Assert
            sut.WithRouteInfo("Error", "BadRequest").ShouldGenerateUrl("/Error/BadRequest");
        }

        [Test]
        public void LoginRequired_ShouldReturnCorrectOutboundRoute()
        {
            // Assemble
            var sut = new RouteTester<MvcApplication>(typeof(RouteConfig));
            // Act and Assert
            sut.WithRouteInfo("Error", "LoginRequired").ShouldGenerateUrl("/Error/LoginRequired");
        }

        [Test]
        public void NotFound_ShouldReturnCorrectOutboundRoute()
        {
            // Assemble
            var sut = new RouteTester<MvcApplication>(typeof(RouteConfig));
            // Act and Assert
            sut.WithRouteInfo("Error", "NotFound").ShouldGenerateUrl("/Error/NotFound");
        }

        [Test]
        public void PermissionDenied_ShouldReturnCorrectOutboundRoute()
        {
            // Assemble
            var sut = new RouteTester<MvcApplication>(typeof(RouteConfig));
            // Act and Assert
            sut.WithRouteInfo("Error", "PermissionDenied").ShouldGenerateUrl("/Error/PermissionDenied");
        }

        [Test]
        public void ServerError_ShouldReturnCorrectOutboundRoute()
        {
            // Assemble
            var sut = new RouteTester<MvcApplication>(typeof(RouteConfig));
            // Act and Assert
            sut.WithRouteInfo("Error", "ServerError").ShouldGenerateUrl("/Error/ServerError");
        }
    }
}
