using System;
using TechTalk.SpecFlow;
using OLO_API.Models;
using TechTalk.SpecFlow.Assist;

namespace OLO_API.Steps
{

    [Binding]
    public class Post_CommentSteps
    {

        GeneralSteps generalSteps;

        //GeneralSteps is provided by context injection, a default functionality of specflow
        //documentation: https://docs.specflow.org/projects/specflow/en/latest/Bindings/Context-Injection.html
        public Post_CommentSteps(GeneralSteps generalSteps)
        {
            this.generalSteps = generalSteps;
        }
        [Given(@"comment object has been loaded with:")]
        public void GivenCommentObjectHasBeenLoadedWith(Table table)
        {
            this.generalSteps.Object = table.CreateInstance<Comment>();
        }

    }
}
