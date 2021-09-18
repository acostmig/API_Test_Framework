using System;
using System.Collections.Generic;
using System.Text;

namespace OLO_API.Models
{
    public class Post : Model
    {
        public int? userId { get; set; }
        public int? id { get; set; }
        public string? title { get; set; }
        public string? body { get; set; }

    }
}
