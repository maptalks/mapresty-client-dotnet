using MockHttpServer;
using System.Collections.Generic;
using System.Net;
using System.Web;

namespace MapResty.Client.Tests.Helper
{
    static class Extensions
    {
        public static Dictionary<string, string> GetFormData(this HttpListenerRequest req)
        {
            var dict = new Dictionary<string, string>();
            var content = req.Content();
            if (content == null)
            {
                return dict;
            }
            var parts = content.Split('&');
            foreach (var part in parts)
            {
                var kv = part.Split('=');
                var k = kv[0];
                var v = HttpUtility.UrlDecode(kv[1]);
                dict.Add(k, v);
            }
            return dict;
        }
    }
}
