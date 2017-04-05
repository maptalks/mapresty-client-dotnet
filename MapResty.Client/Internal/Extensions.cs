using System;
using MapResty.Client.Types;
using RestSharp;
using Newtonsoft.Json;

namespace MapResty.Client.Internal
{
    static class Extensions
    {
        public static IRestRequest AddParametersFromQueryFilter(this RestRequest request, QueryFilter filter)
        {
            var condition = filter.Condition;
            if (!String.IsNullOrWhiteSpace(condition))
            {
                request.AddParameter("condition", condition);
            }

            var spatialFilter = filter.SpatialFilter;
            if (spatialFilter != null && spatialFilter.Geometry != null)
            {
                request.AddParameter("spatialFilter", JsonConvert.SerializeObject(spatialFilter));
            }

            var crs = filter.CRS;
            if (crs != null)
            {
                request.AddParameter("crs", JsonConvert.SerializeObject(crs));
            }

            var fields = filter.ResultFields;
            if (fields != null)
            {
                request.AddParameter("fields", String.Join(",", fields));
            }

            request.AddParameter("returnGeometry", filter.ReturnGeometry);

            return request;
        }
    }
}
