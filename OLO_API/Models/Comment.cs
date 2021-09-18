using System;
using System.Collections.Generic;
using System.Text;

namespace OLO_API.Models
{
    public class Comment : Model
    {
        public int? postId { get; set; }
        public int? id { get; set; }
        public string? name { get; set; }
        public string? email { get; set; }
        public string? body { get; set; }


    }
}
