using Microsoft.VisualStudio.TestTools.UnitTesting;
using MockHttpServer;
using System.Collections.Generic;

namespace MapResty.Client.Tests
{
    [TestClass()]
    public class Global
    {
        public static int port = 11215;
        public static MockServer mockServer;

        [AssemblyInitialize()]
        public static void Init(TestContext context)
        {
            var defaultHandlers = new List<MockHttpHandler>()
            {
                new MockHttpHandler("/", (req, res, param) => {
                })
            };
            mockServer = new MockServer(port, defaultHandlers);
        }

        [AssemblyCleanup()]
        public static void Cleanup()
        {
            mockServer.Dispose();
        }
    }
}
