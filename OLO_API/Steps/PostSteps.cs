using System;
using TechTalk.SpecFlow;
using OLO_API.Drivers;
using System.Net.Http;
using Xunit;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using TechTalk.SpecFlow.Assist;
using OLO_API.Models;

namespace OLO_API.Steps
{
    [Binding]
    public class PostSteps
    {
        GeneralSteps generalSteps;
        public PostSteps(GeneralSteps generalSteps)
        {
            this.generalSteps = generalSteps;
        }

        [Given(@"post object has been loaded with:")]
        public void GivenPostObjectHasBeenLoadedWith(Table table)
        {
            this.generalSteps.Object = table.CreateInstance<Models.Post>();
        }

    }
}
