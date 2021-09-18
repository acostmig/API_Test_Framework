using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace OLO_API.Drivers
{
    /// <summary>
    /// RestAPI manager
    /// </summary>
    public static class RestAPI
    {
        //this should come from an environment variable
        public const string HOST = "https://jsonplaceholder.typicode.com/";


        //overloaded method intended for any Method that doens't have a body
        public static HttpResponseMessage Execute(String method, String Path, Dictionary<string, string> headers = null)
        {
            return Execute(method, Path, null, headers);
        }


        public static HttpResponseMessage Execute(String method, String Path, object body, Dictionary<string, string> headers = null)
        {
            Uri URI;

            //if the provided path already contains a host, create the URI from the path alone
            if (Path.StartsWith("http"))
            {
                URI = new Uri(Path);
            }
            //else, joins the HOST & the provided path and creates the URI
            else
            {
                URI = new Uri(new Uri(HOST), Path);

            }

            HttpClient client = new HttpClient();
            //Default Headers
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //if any headers were provided, set provided headers
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    client.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            }


            HttpRequestMessage request;

            //if body wasn't provided, create Request message without content
            if (body==null)
            {
                request = new HttpRequestMessage(new HttpMethod(method), URI);
                
            }
            //else, create Request message with the provided body
            //we are assuming all body is JSON
            else
            {
                request = new HttpRequestMessage(new HttpMethod(method), URI);
                request.Content = new StringContent(Stringify_if_Object(body), Encoding.UTF8, "application/json");
            }

            
            Log.Info($"Executing [{method}] {URI.AbsoluteUri}");
            
            //
            //Actual Execution of the Request
            //
            HttpResponseMessage response = client.SendAsync(request).Result;


            client.Dispose();
            return response;

        }


        /// <summary>
        /// Converts any object into a JSON string
        /// if a string was provided, return as is
        /// </summary>
        private static string Stringify_if_Object(object obj)
        {
            if (obj is String)
            {
                return (string)obj;
            }
            else
            {
                return JsonConvert.SerializeObject(obj);
            }
        }
    }
}
