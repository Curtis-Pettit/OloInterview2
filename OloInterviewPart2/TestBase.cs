using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;
using System;
using System.Collections.Generic;
using System.Net;

namespace OloInterviewPart2
{
    [TestClass]
    public abstract class TestBase
    {
        protected static TestContext TestContext;
        public static RestClient RestClient { get; private set; }

        [AssemblyInitialize]
        public static void AssemlbyInitialize(TestContext testContext)
        {
            RestClient = new RestClient("https://jsonplaceholder.typicode.com/posts");

            TestContext = testContext;
        }

        protected RestRequest GetRequest(string postId)
        {
            RestRequest request = new RestRequest
            {
                Resource = postId
            };
            RestClient.UseNewtonsoftJson();
            return request;
        }

        protected void LogGetResult(IRestResponse<Post> result)
        {
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.IsNotNull(result.Data, $"Failed to deserailize. Content was: {result.Content}");
            TestContext.WriteLine($"Get returned: {Environment.NewLine}" + result.Data);
        }

        protected void LogGetResult(IRestResponse<List<Post>> result)
        {
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.IsNotNull(result.Data, $"Failed to deserailize. Content was: {result.Content}");
            TestContext.WriteLine($"Get returned: {Environment.NewLine}" + string.Join(Environment.NewLine, result.Data));
        }
    }
}
