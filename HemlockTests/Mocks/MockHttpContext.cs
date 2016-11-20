using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using System.Web.Routing;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Web.SessionState;
using System.Reflection;

namespace HemlockTests.Mocks
{
    class MockHttpContext
    {
        public Mock<HttpContextBase> HttpContextBase { get; private set; }

        public Mock<RequestContext> RoutingRequestContext { get; private set; }
        public Mock<HttpServerUtilityBase> Server { get; private set; }
        public Mock<HttpResponseBase> Response { get; private set; }
        public Mock<HttpRequestBase> Request { get; private set; }
        public Mock<HttpSessionStateBase> Session { get; private set; }
        public Mock<ActionExecutingContext> ActionExecuting { get; private set; }
        public HttpCookieCollection Cookies { get; private set; }

        /*
        Note:
        This constructor will only generate the most barebones HttpContextBase. Session information,
        User Authentication, Cookies, Request/Response need to be setup on an as-needed basis.
        */
        public MockHttpContext()
        {
            RoutingRequestContext = new Mock<RequestContext>(MockBehavior.Loose);
            ActionExecuting = new Mock<ActionExecutingContext>(MockBehavior.Loose);
            HttpContextBase = new Mock<HttpContextBase>(MockBehavior.Loose);
            Server = new Mock<HttpServerUtilityBase>(MockBehavior.Loose);
            Response = new Mock<HttpResponseBase>(MockBehavior.Loose);
            Request = new Mock<HttpRequestBase>(MockBehavior.Loose);
            Session = new Mock<HttpSessionStateBase>(MockBehavior.Loose);
            Cookies = new HttpCookieCollection();

            SetContextProperties();
        }
        
        /*
        Note: 
        Method needs to be called after updating the HttpContext properties or else they will not apply.
        */
        public void SetContextProperties()
        {
            RoutingRequestContext.SetupGet(c => c.HttpContext).Returns(HttpContextBase.Object);
            Request.Setup(c => c.Cookies).Returns(Cookies);

            //Request.Setup(c => c.ApplicationPath).Returns("/testpath/");
            //Response.Setup(c => c.ApplyAppPathModifier(It.IsAny<string>())).Returns("/testVirtualPath/");

            Response.Setup(c => c.Cookies).Returns(Cookies);
            HttpContextBase.SetupGet(c => c.Request).Returns(Request.Object);
            HttpContextBase.SetupGet(c => c.Response).Returns(Response.Object);
            HttpContextBase.SetupGet(c => c.Server).Returns(Server.Object);
            HttpContextBase.SetupGet(c => c.Session).Returns(Session.Object);
            ActionExecuting.SetupGet(c => c.HttpContext).Returns(HttpContextBase.Object);
            //Response.SetupGet(c => c.Headers).Returns(new System.Net.WebHeaderCollection());
        }
        
        /*
            This method is used to return an HttpContext for use outside of controllers. Use the Mock HttpContextBase for
            mocking with Controllers.
        */
        public static HttpContext GenerateBarebonesHttpContext(string requestURL)
        {
            var httpRequest = new HttpRequest("", requestURL, "");
            var stringWriter = new StringWriter();
            var httpResponse = new HttpResponse(stringWriter);
            var httpContext = new HttpContext(httpRequest, httpResponse);

            var sessionContainer = new HttpSessionStateContainer(
                "id",
                new SessionStateItemCollection(),
                new HttpStaticObjectsCollection(),
                10,
                true,
                HttpCookieMode.AutoDetect,
                SessionStateMode.InProc,
                false);

            httpContext.Items["AspSession"] =
                typeof(HttpSessionState).GetConstructor(
                    BindingFlags.NonPublic | BindingFlags.Instance,
                    null,
                    CallingConventions.Standard,
                    new[] { typeof(HttpSessionStateContainer) },
                    null).Invoke(new object[] { sessionContainer });

            return httpContext;
        }

        public static UrlHelper GetUrlHelper(string appPath = "/", RouteCollection routes = null)
        {
            if (routes == null)
            {
                routes = new RouteCollection();
                Hemlock.RouteConfig.RegisterRoutes(routes);
            }

            MockHttpContext mockHttpContext = new MockHttpContext();
            mockHttpContext.Request.Setup(x => x.ApplicationPath).Returns(appPath);

            HttpContextBase httpContext = mockHttpContext.HttpContextBase.Object;
            RouteData routeData = new RouteData();
            routeData.Values.Add("controller", "defaultcontroller");
            routeData.Values.Add("action", "defaultaction");
            RequestContext requestContext = new RequestContext(httpContext, routeData);
            UrlHelper helper = new UrlHelper(requestContext, routes);
            return helper;
        }
    }
}
