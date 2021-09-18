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
    public class GeneralSteps
    {
        /// <summary>
        /// This is the primary object of execution
        /// This object's type will be used as reference for casting incoming objects
        /// </summary>
        public dynamic Object;

        public HttpResponseMessage response;
        public string path;
        public string method;
        public dynamic originalObject;
        public int newlyCreatedId;

        /// <summary>
        /// in the scenario that we needed to save the original prior an update or deletion
        /// </summary>
        [Given(@"original object is loaded performing ""(.*)"" from ""(.*)""")]
        public void GivenOriginalObjectIsLoadedPerformingMETHODFromPATH(string method, string path)
        {
            var response = RestAPI.Execute(method, path);
            this.originalObject = parsePostString(response.Content.ReadAsStringAsync().Result);

        }


        /// <summary>
        /// General perform RestAPI Call Function
        /// </summary>
        [When(@"user performs ""(.*)"" from ""(.*)""")]
        public void WhenUserPerformsMETHODFrom(string method, string path)
        {
            this.method = method;
            //consumes the injected variable newlyCreatedId
            path = path.Replace("{newlyCreatedId}", newlyCreatedId.ToString());
            this.path = path;
             
            this.response = RestAPI.Execute(method, path, Object);


        }
        
        /// <summary>
        /// Asserts the previously executed request has the provided Status Code
        /// </summary>
        [Then(@"the status code should be (.*)")]
        public void ThenTheResponseCodeShouldBe(int expectedStatusCode)
        {
            try
            {
                Assert.Equal(expectedStatusCode, (int)response.StatusCode);
            }
            catch (Exception)
            {
                Log.Error($"response status code [{method}] Path: {path} was unexpected: {(int)response.StatusCode} expected: {expectedStatusCode}");
                Log.Error($"Content: { response.Content.ReadAsStringAsync().Result}");

                //using just throw in order to keep the same stack trace
                throw;
            }
        }
        
        [Then(@"response body should be a valid JSON object")]
        public void ThenResponseBodyShouldBeAValidJSONObject()
        {
            //Throw Exception if Status Code is not 2xx or 3xx
            response.EnsureSuccessStatusCode();

            //read the response as a raw string
            var rawResponse = response.Content.ReadAsStringAsync().Result;

            //convert the response into a JSON object
            //This would fail if JSON was formatted incorrectly
            JsonConvert.DeserializeObject<JToken>(rawResponse);

        }

        [Then(@"response body should be a valid object")]
        public void ThenResponseBodyShouldBeAValidPostObject()
        {
            //Throw Exception if Status Code is not 2xx or 3xx
            response.EnsureSuccessStatusCode();

            //read the response as a raw string
            var rawResponse = response.Content.ReadAsStringAsync().Result;

            //parse the response into the Primary Type under execution
            var post = parsePostString(rawResponse);

            //in case the method is a post (creation) we will save the newly created ID for later use
            if (this.method == "POST")
            {
                this.newlyCreatedId = post.id ?? 0;
            }
        }

        [Then(@"response body should match the provided object")]
        public void ThenResponseBodyShouldMatchTheProvidedObject()
        {
            //Throw Exception if Status Code is not 2xx or 3xx
            response.EnsureSuccessStatusCode();

            //read the response as a raw string
            var rawResponse = response.Content.ReadAsStringAsync().Result;

            //parse the response into the Primary Type under execution
            dynamic actualObj = parsePostString(rawResponse);

            AssertObjectsMatch(this.Object, actualObj);
        }

        /// <summary>
        /// This Function is the same as ThenResponseBodyShouldMatchTheProvidedObject()
        /// Except it doesn't check for the ID property
        /// </summary>
        [Then(@"response body should match the provided object data")]
        public void ThenResponseBodyShouldMatchTheProvidedObjectData()
        {
            //Throw Exception if Status Code is not 2xx or 3xx
            response.EnsureSuccessStatusCode();
            var rawResponse = response.Content.ReadAsStringAsync().Result;
            dynamic actualObj = parsePostString(rawResponse);

            //note last parameter is true, is the flag not to check the ID property
            AssertObjectsMatch(this.Object, actualObj, true);
        }
        [Then(@"response body should match original object except for the provided properties")]
        public void ThenResponseBodyShouldMatchOriginalObjectExceptForTheProvidedProperties()
        {
            //Throw Exception if Status Code is not 2xx or 3xx
            response.EnsureSuccessStatusCode();

            //read the response as a raw string
            var rawResponse = response.Content.ReadAsStringAsync().Result;

            //parse the response into the Primary Type under execution
            dynamic actualObj = parsePostString(rawResponse);

            //create expected object as an empty instance of the Primary Type under execution
            dynamic expectedObject = Activator.CreateInstance(this.Object.GetType());

            /**
             * 
             * 
             *  1.)Loop through each property in the Primary Type under execution
             *      2.) load the original value for the property under context
             *      3.) load the provided value for the property under context
             *      4.) if the provided object's property under context was null or the name is ID
             *           5.) invoke the set method for the expected object,
             *               Expected property now becomes the original (null meaning it as not provided & ID must be unchanged)
             *               
             *      6.) else
             *           7.) meaning the provided object's property was loaded,
             *               Expected property now becomes the provided
             *          
             *               
             * 
            **/

            foreach (var prop in ((Type)originalObject.GetType()).GetProperties())
            {
                var originalObject_propValue = prop.GetGetMethod().Invoke(originalObject, null);
                var providedObject_propValue = prop.GetGetMethod().Invoke(Object, null);
                

                if (providedObject_propValue == null || prop.Name.ToLower() == "id")
                {
                    _ = prop.GetSetMethod().Invoke(expectedObject, new object[] { originalObject_propValue });
                }
                else
                {
                    _ = prop.GetSetMethod().Invoke(expectedObject, new object[] { providedObject_propValue });

                }
            }

            //finally, assert the result matches the expected object we got out of the loop above
            AssertObjectsMatch(expectedObject, actualObj);
        }


        public dynamic parsePostString(string jsonString)
        {
            try
            {
                //By using the primary object's type, we will parse the given JSON string into that object
                return JsonConvert.DeserializeObject(jsonString, (Type)this.Object.GetType());
            }
            catch (Exception)
            {
                Log.Error($"JSON object for [{method}] Path: {path} was invalid \n{jsonString} ");


                //using just throw in order to keep the same stack trace
                throw;
            }
        }

        /// <summary>
        /// This Function will assert that two objects of any tipe are exactly the same
        /// 
        /// requires the provided objects to have a valid overwrite to ToString()
        /// </summary>
        public void AssertObjectsMatch(dynamic expectedObject, dynamic actualObject, bool ignoreId = false)
        {
            try
            {
                //flag to ignore IDs (in case we are testing data only)
                if(ignoreId)
                {
                    expectedObject.id = null;
                    actualObject.id = null;
                }
                Assert.Equal(expectedObject.ToString(), actualObject.ToString());
            }
            catch (Exception)
            {
                Log.Error($"returned object from [{method}] Path: {path} does not match provided object \n Expected & Actual: \n{expectedObject}\n{actualObject} ");


                //using just throw in order to keep the same stack trace
                throw;
            }

            Log.Info($"Expected & Actual: \n{expectedObject}\n{actualObject}");
        }

        
    }
}
