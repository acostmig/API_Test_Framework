using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace OLO_API.Models
{
    public abstract class Model
    {
        public override string ToString()
        {
            return JObject.FromObject(this).ToString();
        }
    }
}
