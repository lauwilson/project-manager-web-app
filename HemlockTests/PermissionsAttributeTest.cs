using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hemlock.Controllers.ActionFilters;
using Hemlock.Models.Enum;
using Hemlock.Models;
using NUnit.Framework;
using System.Web.Mvc;
using System.Web;
using System.Web.Routing;
using HemlockTests.Mocks;
using Moq;
using System.Security.Authentication;

namespace HemlockTests
{
    [TestFixture]
    class PermissionsAttributeTest
    {
        [Test]
        public void PermissionsAttribute_ShouldRedirectRequestToLoginRequiredPage_IfUserDoesNotExistInSession()
        {
            // Arrange
            // HttpContext.Current
            HttpContext.Current = MockHttpContext.GenerateBarebonesHttpContext("http://localhost/MyActivity");

            var mockHttpContext = new MockHttpContext();
            ActionExecutingContext filterContext = new ActionExecutingContext();
            filterContext.HttpContext = mockHttpContext.HttpContextBase.Object;
            PermissionsAttribute sut = new PermissionsAttribute(PermissionsEnum.CanViewOwnActivity);

            //Act
            sut.OnActionExecuting(filterContext);

            //Assert
            Assert.That(filterContext.Result, Is.InstanceOf<RedirectToRouteResult>());
            RedirectToRouteResult result = (RedirectToRouteResult)filterContext.Result;
            Assert.AreEqual(result.RouteValues["controller"], "Error");
            Assert.AreEqual(result.RouteValues["action"], "LoginRequired");
        }

        [Test]
        public void PermissionsAttribute_ShouldNotThrowException_IfUserHasRequiredPermissions()
        {
            // Arrange
            HttpContext.Current = MockHttpContext.GenerateBarebonesHttpContext("http://localhost/MyActivity");
            Employee mockEmployee = new Employee { Permissions = (int)PermissionsEnum.CanViewOwnActivity };
            HttpContext.Current.Session["User"] = mockEmployee;

            Mock<ActionExecutingContext> mockFilterContext = new Mock<ActionExecutingContext>();
            var mockHttpContext = new Mock<HttpContextBase>();
            PermissionsAttribute sut = new PermissionsAttribute(PermissionsEnum.CanViewOwnActivity);

            ControllerContext Controller = new ControllerContext { HttpContext = mockHttpContext.Object };
            
            // Act
            // Assert
            Assert.DoesNotThrow(() => sut.OnActionExecuting(mockFilterContext.Object));
        }

        [Test]
        public void PermissionsAttribute_ShouldRedirectRequestToPermissionsDeniedPage_IfEmployeeDoesNotHaveRequiredPermissions()
        {
            // Arrange
            HttpContext.Current = MockHttpContext.GenerateBarebonesHttpContext("http://localhost/MyActivity");
            Employee mockEmployee = new Employee { Permissions = (int)PermissionsEnum.CanViewOwnActivity };
            HttpContext.Current.Session["User"] = mockEmployee;

            ActionExecutingContext filterContext = new ActionExecutingContext();
            PermissionsAttribute sut = new PermissionsAttribute(PermissionsEnum.CanDeleteActivity);

            // Act
            sut.OnActionExecuting(filterContext);

            // Assert
            Assert.That(filterContext.Result, Is.InstanceOf<RedirectToRouteResult>());
            RedirectToRouteResult result = (RedirectToRouteResult)filterContext.Result;
            Assert.AreEqual(result.RouteValues["controller"], "Error");
            Assert.AreEqual(result.RouteValues["action"], "PermissionDenied");
        }
    }
}
