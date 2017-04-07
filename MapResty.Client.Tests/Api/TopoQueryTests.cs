using GeoJSON.Net.Geometry;
using MapResty.Client.Internal;
using MapResty.Client.Tests.Helper;
using MapResty.Client.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MockHttpServer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MapResty.Client.Api.Tests
{
    [TestClass()]
    public class TopoQueryTests
    {
        private static int port = 11215;
        private static string urlPrefix = "/rest/geometry";
        private static MockServer mockServer;

        [ClassInitialize()]
        public static void ClassInit(TestContext context)
        {
            var defaultHandlers = new List<MockHttpHandler>()
            {
                new MockHttpHandler("/", (req, res, param) => {
                })
            };
            mockServer = new MockServer(port, defaultHandlers);
        }

        [ClassCleanup()]
        public static void ClassCleanup()
        {
        }

        [TestInitialize()]
        public void TestInit()
        {
        }

        [TestCleanup()]
        public void TestCleanup()
        {
            mockServer.ClearRequestHandlers();
        }

        [TestMethod()]
        public void RelateTest()
        {
            var source = new Point(new GeographicPosition(30.0, 110.0));
            var targets = new IGeometryObject[]
            {
                new Point(new GeographicPosition(30.0, 110.0))
            };
            var relation = SpatialFilter.RELATION_CONTAIN;

            var url = String.Join("/", new string[] { urlPrefix, "relation" });
            var handler = new MockHttpHandler(url, "POST", (req, res, param) =>
            {
                var result = new RestResult();
                var form = req.GetFormData();
                if (form.ContainsKey("source") && form.ContainsKey("targets") && form.ContainsKey("relation"))
                {
                    result.Success = true;
                }
                return JsonConvert.SerializeObject(result);
            });
            mockServer.AddRequestHandler(handler);

            try
            {
                var topo = new TopoQuery();
                topo.Relate(source, targets, relation);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod()]
        public void IntersectionTest()
        {
            var source = new Point(new GeographicPosition(30.0, 110.0));
            var targets = new IGeometryObject[]
            {
                new Point(new GeographicPosition(30.0, 110.0))
            };

            var url = String.Join("/", new string[] { urlPrefix, "analysis/intersection" });
            var handler = new MockHttpHandler(url, "POST", (req, res, param) =>
            {
                var result = new RestResult();
                var form = req.GetFormData();
                if (form.ContainsKey("source") && form.ContainsKey("targets"))
                {
                    result.Success = true;
                }
                return JsonConvert.SerializeObject(result);
            });
            mockServer.AddRequestHandler(handler);

            try
            {
                var topo = new TopoQuery();
                topo.Intersection(source, targets);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod()]
        public void SimplifyTest()
        {
            var input = new IGeometryObject[]
            {
                new Point(new GeographicPosition(30.0, 110.0))
            };

            var url = String.Join("/", new string[] { urlPrefix, "simplify" });
            var handler = new MockHttpHandler(url, "POST", (req, res, param) =>
            {
                var result = new RestResult();
                var form = req.GetFormData();
                if (form.ContainsKey("targets"))
                {
                    result.Data = JsonConvert.SerializeObject(input);
                    result.Success = true;
                }
                return JsonConvert.SerializeObject(result);
            });
            mockServer.AddRequestHandler(handler);

            try
            {
                var topo = new TopoQuery();
                var result = topo.Simplify(input);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod()]
        public void BufferTest()
        {
            var input = new IGeometryObject[]
            {
                new Point(new GeographicPosition(30.0, 110.0))
            };

            var url = String.Join("/", new string[] { urlPrefix, "analysis/buffer" });
            var handler = new MockHttpHandler(url, "POST", (req, res, param) =>
            {
                var result = new RestResult();
                var form = req.GetFormData();
                if (form.ContainsKey("targets") && form.ContainsKey("distance"))
                {
                    result.Data = JsonConvert.SerializeObject(input);
                    result.Success = true;
                }
                return JsonConvert.SerializeObject(result);
            });
            mockServer.AddRequestHandler(handler);

            try
            {
                var topo = new TopoQuery();
                var result = topo.Buffer(input, 100);
                Assert.AreEqual(input.Length, result.Length);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod()]
        public void ConvexHullTest()
        {
            var input = new IGeometryObject[]
            {
                new Point(new GeographicPosition(30.0, 110.0))
            };

            var url = String.Join("/", new string[] { urlPrefix, "analysis/convexhull" });
            var handler = new MockHttpHandler(url, "POST", (req, res, param) =>
            {
                var result = new RestResult();
                var form = req.GetFormData();
                if (form.ContainsKey("targets"))
                {
                    result.Data = JsonConvert.SerializeObject(input);
                    result.Success = true;
                }
                return JsonConvert.SerializeObject(result);
            });
            mockServer.AddRequestHandler(handler);

            try
            {
                var topo = new TopoQuery();
                var result = topo.ConvexHull(input);
                Assert.AreEqual(input.Length, result.Length);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod()]
        public void DifferenceTest()
        {
            var source = new Point(new GeographicPosition(30.0, 110.0));
            var targets = new IGeometryObject[]
            {
                new Point(new GeographicPosition(30.0, 110.0))
            };
            var relation = SpatialFilter.RELATION_CONTAIN;

            var url = String.Join("/", new string[] { urlPrefix, "analysis/difference" });
            var handler = new MockHttpHandler(url, "POST", (req, res, param) =>
            {
                var result = new RestResult();
                var form = req.GetFormData();
                if (form.ContainsKey("source") && form.ContainsKey("targets"))
                {
                    result.Data = JsonConvert.SerializeObject(targets);
                    result.Success = true;
                }
                return JsonConvert.SerializeObject(result);
            });
            mockServer.AddRequestHandler(handler);

            try
            {
                var topo = new TopoQuery();
                var result = topo.Difference(source, targets);
                Assert.AreEqual(targets.Length, result.Length);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod()]
        public void SymDifferenceTest()
        {
            var source = new Point(new GeographicPosition(30.0, 110.0));
            var targets = new IGeometryObject[]
            {
                new Point(new GeographicPosition(30.0, 110.0))
            };

            var url = String.Join("/", new string[] { urlPrefix, "analysis/symdifference" });
            var handler = new MockHttpHandler(url, "POST", (req, res, param) =>
            {
                var result = new RestResult();
                var form = req.GetFormData();
                if (form.ContainsKey("source") && form.ContainsKey("targets"))
                {
                    result.Data = JsonConvert.SerializeObject(targets);
                    result.Success = true;
                }
                return JsonConvert.SerializeObject(result);
            });
            mockServer.AddRequestHandler(handler);

            try
            {
                var topo = new TopoQuery();
                var result = topo.SymDifference(source, targets);
                Assert.AreEqual(targets.Length, result.Length);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod()]
        public void UnionTest()
        {
            var source = new Point(new GeographicPosition(30.0, 110.0));
            var targets = new IGeometryObject[]
            {
                new Point(new GeographicPosition(30.0, 110.0))
            };

            var url = String.Join("/", new string[] { urlPrefix, "analysis/union" });
            var handler = new MockHttpHandler(url, "POST", (req, res, param) =>
            {
                var result = new RestResult();
                var form = req.GetFormData();
                if (form.ContainsKey("source") && form.ContainsKey("targets"))
                {
                    result.Data = JsonConvert.SerializeObject(targets);
                    result.Success = true;
                }
                return JsonConvert.SerializeObject(result);
            });
            mockServer.AddRequestHandler(handler);

            try
            {
                var topo = new TopoQuery();
                var result = topo.Union(source, targets);
                Assert.AreEqual(targets.Length, result.Length);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}
