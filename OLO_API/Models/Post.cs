using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace OLO_API.Models
{
    public class Post
    {
        public int? userId { get; set; }
        public int? id { get; set; }
        public string? title { get; set; }
        public string? body { get; set; }

        public override string ToString()
        {
            return JObject.FromObject(this).ToString();
        }
    }
}
