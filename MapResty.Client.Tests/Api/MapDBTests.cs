using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using MapResty.Client.Internal;
using MapResty.Client.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MockHttpServer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MapResty.Client.Api.Tests
{
    [TestClass()]
    public class MapDBTests
    {
        private static int port = 11215;
        private static string urlPrefix = "/rest/sdb/database";
        private static string db1 = "db1";
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
        public void InstallSuccessTest()
        {
            var url = String.Join("/", new string[] { urlPrefix, db1 });
            var handler = new MockHttpHandler(url, "POST", (req, res, param) =>
            {
                var result = new RestResult();
                result.Success = true;
                return JsonConvert.SerializeObject(result);
            });
            mockServer.AddRequestHandler(handler);

            try
            {
                var db = new MapDB(db1);
                db.Install(null);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod()]
        public void InstallFailedTest()
        {
            var url = String.Join("/", new string[] { urlPrefix, db1 });
            var handler = new MockHttpHandler(url, "POST", (req, res, param) =>
            {
                var result = new RestResult();
                result.Success = false;
                return JsonConvert.SerializeObject(result);
            });
            mockServer.AddRequestHandler(handler);

            try
            {
                var db = new MapDB(db1);
                db.Install(null);
                Assert.Fail();
            }
            catch
            {
                // pass
            }
        }

        [TestMethod()]
        public void GetDbInfoTest()
        {
            var expected = new DbInfo();
            expected.Name = db1;
            expected.Version = "1.0.0";
            expected.CRS = CRS.WGS84;

            var url = String.Join("/", new string[] { urlPrefix, db1 });
            var handler = new MockHttpHandler(url, "GET", (req, res, param) =>
            {
                var result = new RestResult();
                result.Success = true;
                result.Data = JsonConvert.SerializeObject(expected);

                return JsonConvert.SerializeObject(result);
            });
            mockServer.AddRequestHandler(handler);

            try
            {
                var db = new MapDB(db1);
                var actual = db.GetDbInfo();
                Assert.AreEqual<DbInfo>(expected, actual);
            }
            catch
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void GetAllLayersTest()
        {
            var id = "id";
            var expected = new List<Layer>()
            {
                new Layer()
                {
                    Id = id,
                    Type = Layer.TYPE_DB_TABLE,
                    Name = "name",
                    Source = "source",
                    Fields = new List<LayerField>()
                    {
                        new LayerField()
                        {
                            FieldName = "id",
                            DataType = "UUID",
                            FieldSize = 64
                        }
                    }
                }
            };

            var url = String.Join("/", new string[] { urlPrefix, db1, "layers" });
            var handler = new MockHttpHandler(url, "GET", (req, res, param) =>
            {
                var result = new RestResult();
                result.Success = true;
                result.Data = JsonConvert.SerializeObject(expected);

                return JsonConvert.SerializeObject(result);
            });
            mockServer.AddRequestHandler(handler);

            try
            {
                var db = new MapDB(db1);
                var actual = db.GetAllLayers();
                Assert.AreEqual<Layer>(expected[0], actual[0]);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod()]
        public void GetLayerTest()
        {
            var id = "id";
            var expected = new Layer()
            {
                Id = id,
                Type = Layer.TYPE_DB_TABLE,
                Name = "name",
                Source = "source",
                Fields = new List<LayerField>()
                {
                    new LayerField()
                    {
                        FieldName = "id",
                        DataType = "UUID",
                        FieldSize = 64
                    }
                }
            };

            var url = String.Join("/", new string[] { urlPrefix, db1, "layers", id });
            var handler = new MockHttpHandler(url, "GET", (req, res, param) =>
            {
                var result = new RestResult();
                result.Success = true;
                result.Data = JsonConvert.SerializeObject(expected);

                return JsonConvert.SerializeObject(result);
            });
            mockServer.AddRequestHandler(handler);

            try
            {
                var db = new MapDB(db1);
                var actual = db.GetLayer(id);
                Assert.AreEqual<Layer>(expected, actual);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod()]
        public void AddLayerTest()
        {
            var id = "id";
            var layer = new Layer()
            {
                Id = id,
                Type = Layer.TYPE_DB_TABLE,
                Name = "name",
                Source = "source",
                Fields = new List<LayerField>()
                {
                    new LayerField()
                    {
                        FieldName = "id",
                        DataType = "UUID",
                        FieldSize = 64
                    }
                }
            };

            var url = String.Join("/", new string[] { urlPrefix, db1, "layers" });
            var handler = new MockHttpHandler(url, "POST", (req, res, param) =>
            {
                var result = new RestResult();
                result.Success = true;
                return JsonConvert.SerializeObject(result);
            });
            mockServer.AddRequestHandler(handler);

            try
            {
                var db = new MapDB(db1);
                db.AddLayer(layer);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod()]
        public void RemoveLayerTest()
        {
            var id = "id";

            var url = String.Join("/", new string[] { urlPrefix, db1, "layers" });
            var handler = new MockHttpHandler(url, "POST", (req, res, param) =>
            {
                var result = new RestResult();
                result.Success = true;
                return JsonConvert.SerializeObject(result);
            });
            mockServer.AddRequestHandler(handler);

            try
            {
                var db = new MapDB(db1);
                db.RemoveLayer(id);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod()]
        public void UpdateLayerTest()
        {
            var oldId = "oldId";
            var newId = "newId";
            var layer = new Layer()
            {
                Id = newId,
                Type = Layer.TYPE_DB_TABLE,
                Name = "name",
                Source = "source",
                Fields = new List<LayerField>()
                {
                    new LayerField()
                    {
                        FieldName = "id",
                        DataType = "UUID",
                        FieldSize = 64
                    }
                }
            };

            var url = String.Join("/", new string[] { urlPrefix, db1, "layers", oldId });
            var handler = new MockHttpHandler(url, "POST", (req, res, param) =>
            {
                var result = new RestResult();
                result.Success = true;
                return JsonConvert.SerializeObject(result);
            });
            mockServer.AddRequestHandler(handler);

            try
            {
                var db = new MapDB(db1);
                db.UpdateLayer(oldId, layer);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod()]
        public void QueryJSONTest()
        {
            var id = "id";
            var point = new Point(new GeographicPosition(30.0, 110.0));
            var feature = new Feature(point);
            var features = new List<Feature>();
            features.Add(feature);
            var collection = new FeatureCollection(features);
            var array = new FeatureCollection[] { collection };
            var expected = JsonConvert.SerializeObject(array);

            var url = String.Join("/", new string[] { urlPrefix, db1, "layers", id, "data" });
            var handler = new MockHttpHandler(url, "POST", (req, res, param) =>
            {
                var result = new RestResult();
                result.Success = true;
                result.Count = 1;
                result.Data = JsonConvert.SerializeObject(array);
                return JsonConvert.SerializeObject(result);
            });
            mockServer.AddRequestHandler(handler);

            try
            {
                var db = new MapDB(db1);
                var filter = new QueryFilter();
                var actual = db.QueryJSON(filter, 0, 10, new string[] { id });
                Assert.AreEqual(expected, actual);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod()]
        public void QueryTest()
        {
            var id = "id";
            var point = new Point(new GeographicPosition(30.0, 110.0));
            var feature = new Feature(point);
            var features = new List<Feature>();
            features.Add(feature);
            var collection = new FeatureCollection(features);
            var expected = new FeatureCollection[] { collection };

            var url = String.Join("/", new string[] { urlPrefix, db1, "layers", id, "data" });
            var handler = new MockHttpHandler(url, "POST", (req, res, param) =>
            {
                var result = new RestResult();
                result.Success = true;
                result.Count = 1;
                result.Data = JsonConvert.SerializeObject(expected);
                return JsonConvert.SerializeObject(result);
            });
            mockServer.AddRequestHandler(handler);

            try
            {
                var db = new MapDB(db1);
                var filter = new QueryFilter();
                var actual = db.Query(filter, 0, 10, new string[] { id });
                Assert.AreEqual(expected[0], actual[0]);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}
