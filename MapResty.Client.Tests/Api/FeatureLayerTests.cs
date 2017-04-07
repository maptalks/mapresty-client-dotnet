using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using MapResty.Client.Internal;
using MapResty.Client.Tests.Helper;
using MapResty.Client.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MockHttpServer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using MapResty.Client.Tests;

namespace MapResty.Client.Api.Tests
{
    [TestClass()]
    public class FeatureLayerTests
    {
        private static string urlPrefix = "/rest/sdb/database";
        private static string db1 = "db1";
        private static string layer1 = "layer1";
        private static string urlPrefix1 = String.Join("/", new string[] { urlPrefix, db1, "layers" });
        private static MockServer mockServer = Global.mockServer;

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
        public void AddLayerFieldTest()
        {
            var url = String.Join("/", new string[] { urlPrefix1, layer1, "fields" });
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
                var layer = new FeatureLayer(layer1, db);
                var field = new LayerField()
                {
                    FieldName = "fieldName",
                    DataType = "INT",
                    FieldSize = 64
                };
                layer.AddLayerField(field);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod()]
        public void UpdateLayerFieldTest()
        {
            var oldName = "fieldName";

            var url = String.Join("/", new string[] { urlPrefix1, layer1, "fields", oldName });
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
                var layer = new FeatureLayer(layer1, db);
                var field = new LayerField()
                {
                    FieldName = "newFieldName",
                    DataType = "INT",
                    FieldSize = 64
                };
                layer.UpdateLayerField(oldName, field);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod()]
        public void RemoveLayerFieldTest()
        {
            var fieldName = "fieldName";

            var url = String.Join("/", new string[] { urlPrefix1, layer1, "fields", fieldName });
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
                var layer = new FeatureLayer(layer1, db);
                layer.RemoveLayerField(fieldName);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod()]
        public void GetLayerFieldsTest()
        {
            var expected = new List<LayerField>()
            {
                new LayerField()
                {
                    FieldName = "fieldName",
                    DataType = "BLOB",
                    FieldSize = 600
                }
            };

            var url = String.Join("/", new string[] { urlPrefix1, layer1, "fields" });
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
                var layer = new FeatureLayer(layer1, db);
                var actual = layer.GetLayerFields();
                Assert.AreEqual<LayerField>(expected[0], actual[0]);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod()]
        public void AddFeatureTest()
        {
            var point = new Point(new GeographicPosition(30.0, 110.0));
            var expected = new Feature(point);

            var url = String.Join("/", new string[] { urlPrefix1, layer1, "data" });
            var handler = new MockHttpHandler(url, "POST", (req, res, param) =>
            {
                var result = new RestResult();

                var form = req.GetFormData();
                if (form.ContainsKey("data"))
                {
                    var data = form["data"];
                    try
                    {

                        var actual = JsonConvert.DeserializeObject<Feature>(data);
                        if (expected == actual)
                        {
                            result.Success = true;
                        }
                    }
                    catch
                    {
                    }
                }

                return JsonConvert.SerializeObject(result);
            });
            mockServer.AddRequestHandler(handler);

            try
            {
                var db = new MapDB(db1);
                var layer = new FeatureLayer(layer1, db);
                layer.Add(expected);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod()]
        public void AddFeatureArrayTest()
        {
            var point1 = new Point(new GeographicPosition(30.0, 110.0));
            var point2 = new Point(new GeographicPosition(30.0, 120.0));
            var expected = new Feature[] 
            { 
                new Feature(point1), 
                new Feature(point2)
            };

            var url = String.Join("/", new string[] { urlPrefix1, layer1, "data" });
            var handler = new MockHttpHandler(url, "POST", (req, res, param) =>
            {
                var result = new RestResult();

                var form = req.GetFormData();
                if (form.ContainsKey("data"))
                {
                    var data = form["data"];
                    try
                    {
                        var actual = JsonConvert.DeserializeObject<Feature[]>(data);
                        if (expected.SequenceEqual(actual))
                        {
                            result.Success = true;
                        }
                    }
                    catch
                    {
                    }
                }

                return JsonConvert.SerializeObject(result);
            });
            mockServer.AddRequestHandler(handler);

            try
            {
                var db = new MapDB(db1);
                var layer = new FeatureLayer(layer1, db);
                layer.Add(expected);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod()]
        public void AddFeatureListTest()
        {
            var point1 = new Point(new GeographicPosition(30.0, 110.0));
            var point2 = new Point(new GeographicPosition(30.0, 120.0));
            var expected = new List<Feature>()
            { 
                new Feature(point1),
                new Feature(point2)
            };

            var url = String.Join("/", new string[] { urlPrefix1, layer1, "data" });
            var handler = new MockHttpHandler(url, "POST", (req, res, param) =>
            {
                var result = new RestResult();

                var form = req.GetFormData();
                if (form.ContainsKey("data"))
                {
                    var data = form["data"];
                    try
                    {
                        var actual = JsonConvert.DeserializeObject<List<Feature>>(data);
                        if (expected.SequenceEqual(actual))
                        {
                            result.Success = true;
                        }
                    }
                    catch
                    {
                    }
                }

                return JsonConvert.SerializeObject(result);
            });
            mockServer.AddRequestHandler(handler);

            try
            {
                var db = new MapDB(db1);
                var layer = new FeatureLayer(layer1, db);
                layer.Add(expected);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod()]
        public void GetFirstTest()
        {
            var point1 = new Point(new GeographicPosition(30.0, 110.0));
            var point2 = new Point(new GeographicPosition(30.0, 120.0));
            var feature1 = new Feature(point1);
            var feature2 = new Feature(point2);
            var features = new List<Feature>()
            { 
                feature1, 
                feature2
            };
            var collections = new List<FeatureCollection>()
            {
                new FeatureCollection(features)
            };

            var url = String.Join("/", new string[] { urlPrefix1, layer1, "data" });
            var handler = new MockHttpHandler(url, "POST", (req, res, param) =>
            {
                var result = new RestResult();
                result.Success = true;
                result.Data = JsonConvert.SerializeObject(collections);
                return JsonConvert.SerializeObject(result);
            });
            mockServer.AddRequestHandler(handler);

            try
            {
                var db = new MapDB(db1);
                var layer = new FeatureLayer(layer1, db);
                var actual = layer.GetFirst("");
                Assert.AreEqual<Feature>(feature1, actual);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod()]
        public void UpdateTest()
        {
            var point = new Point(new GeographicPosition(30.0, 110.0));
            var feature = new Feature(point);

            var url = String.Join("/", new string[] { urlPrefix1, layer1, "data" });
            var handler = new MockHttpHandler(url, "POST", (req, res, param) =>
            {
                var result = new RestResult();
                var form = req.GetFormData();
                if (form.ContainsKey("data") && form.ContainsKey("condition"))
                {
                    result.Success = true;
                }
                return JsonConvert.SerializeObject(result);
            });
            mockServer.AddRequestHandler(handler);

            try
            {
                var db = new MapDB(db1);
                var layer = new FeatureLayer(layer1, db);
                layer.Update("", feature, null);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod()]
        public void RemoveTest()
        {
            var expected = "x = 3";

            var url = String.Join("/", new string[] { urlPrefix1, layer1, "data" });
            var handler = new MockHttpHandler(url, "POST", (req, res, param) =>
            {
                var result = new RestResult();
                var form = req.GetFormData();
                if (form.ContainsKey("condition"))
                {
                    var actual = form["condition"];
                    if (expected == actual)
                    {
                        result.Success = true;
                    }
                }
                return JsonConvert.SerializeObject(result);
            });
            mockServer.AddRequestHandler(handler);

            try
            {
                var db = new MapDB(db1);
                var layer = new FeatureLayer(layer1, db);
                layer.Remove(expected);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod()]
        public void RemoveAllTest()
        {
            var url = String.Join("/", new string[] { urlPrefix1, layer1, "data" });
            var handler = new MockHttpHandler(url, "POST", (req, res, param) =>
            {
                var result = new RestResult();
                if (param["op"] == "removeAll")
                {
                    result.Success = true;
                }
                return JsonConvert.SerializeObject(result);
            });
            mockServer.AddRequestHandler(handler);

            try
            {
                var db = new MapDB(db1);
                var layer = new FeatureLayer(layer1, db);
                layer.Remove();
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod()]
        public void ClearTest()
        {
            RemoveAllTest();
        }

        [TestMethod()]
        public void UpdatePropertiesTest()
        {
            var url = String.Join("/", new string[] { urlPrefix1, layer1, "data" });
            var handler = new MockHttpHandler(url, "POST", (req, res, param) =>
            {
                var result = new RestResult();
                var form = req.GetFormData();
                if (form.ContainsKey("condition") && form.ContainsKey("data"))
                {
                    result.Success = true;
                }
                return JsonConvert.SerializeObject(result);
            });
            mockServer.AddRequestHandler(handler);

            try
            {
                var db = new MapDB(db1);
                var layer = new FeatureLayer(layer1, db);
                layer.UpdateProperties("", null);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod()]
        public void QueryTest()
        {
            var point = new Point(new GeographicPosition(30.0, 110.0));
            var feature = new Feature(point);
            var features = new List<Feature>()
            { 
                feature
            };
            var collections = new List<FeatureCollection>()
            {
                new FeatureCollection(features)
            };

            var url = String.Join("/", new string[] { urlPrefix1, layer1, "data" });
            var handler = new MockHttpHandler(url, "POST", (req, res, param) =>
            {
                var result = new RestResult();
                result.Success = true;
                result.Data = JsonConvert.SerializeObject(collections);
                return JsonConvert.SerializeObject(result);
            });
            mockServer.AddRequestHandler(handler);

            try
            {
                var db = new MapDB(db1);
                var layer = new FeatureLayer(layer1, db);
                var actual = layer.Query(null, 0, 10);
                Assert.IsTrue(features.SequenceEqual(actual));
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod()]
        public void QueryJSONTest()
        {
            var point = new Point(new GeographicPosition(30.0, 110.0));
            var feature = new Feature(point);
            var features = new List<Feature>() { feature };
            var collections = new List<FeatureCollection>()
            {
                new FeatureCollection(features)
            };
            var expected = JsonConvert.SerializeObject(collections);

            var url = String.Join("/", new string[] { urlPrefix1, layer1, "data" });
            var handler = new MockHttpHandler(url, "POST", (req, res, param) =>
            {
                var result = new RestResult();
                result.Success = true;
                result.Data = JsonConvert.SerializeObject(collections);
                return JsonConvert.SerializeObject(result);
            });
            mockServer.AddRequestHandler(handler);

            try
            {
                var db = new MapDB(db1);
                var layer = new FeatureLayer(layer1, db);
                var actual = layer.QueryJSON(null, 0, 10);
                Assert.AreEqual(expected, actual);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod()]
        public void CountTest()
        {
            var expected = 3;

            var url = String.Join("/", new string[] { urlPrefix1, layer1, "data" });
            var handler = new MockHttpHandler(url, "POST", (req, res, param) =>
            {
                var result = new RestResult();
                result.Success = true;
                result.Data = expected;
                return JsonConvert.SerializeObject(result);
            });
            mockServer.AddRequestHandler(handler);

            try
            {
                var db = new MapDB(db1);
                var layer = new FeatureLayer(layer1, db);
                var actual = layer.Count(null);
                Assert.AreEqual(expected, actual);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}
