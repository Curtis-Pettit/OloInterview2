using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;
using System.Collections.Generic;
using System.Net;

namespace OloInterviewPart2
{
    [TestClass]
    public class GetTests : TestBase
    {
        private const string post2Title = "qui est esse";
        private const string post2Body = "est rerum tempore vitae\nsequi sint nihil reprehenderit dolor beatae ea dolores neque\nfugiat blanditiis voluptate porro vel nihil molestiae ut reiciendis\nqui aperiam non debitis possimus qui neque nisi nulla";

        [TestMethod]
        public void GetTest()
        {
            RestRequest request = new RestRequest();
            RestClient.UseNewtonsoftJson();
            var result = RestClient.Get<List<Post>>(request);
            Assert.IsNotNull(result);
            TestContext.WriteLine(result.ToString());

            //TODO no good once i start posting and deleting things
            Assert.AreEqual(100, result.Data.Count);

            Assert.AreEqual(1, result.Data[0].Id);
            Assert.AreEqual(1, result.Data[0].UserId);
            Assert.AreEqual("sunt aut facere repellat provident occaecati excepturi optio reprehenderit", result.Data[0].Title);
            Assert.AreEqual("quia et suscipit\nsuscipit recusandae consequuntur expedita et cum\nreprehenderit molestiae ut ut quas totam\nnostrum rerum est autem sunt rem eveniet architecto",
                result.Data[0].Body);
        }

        [TestMethod]
        public void GetSpecificTest()
        {
            RestRequest request = new RestRequest
            {
                Resource = "2"
            };
            RestClient.UseNewtonsoftJson();

            var result = RestClient.Get<Post>(request);

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.IsNotNull(result.Data);

            TestContext.WriteLine(result.ToString());
            
            Assert.AreEqual(2, result.Data.Id);
            Assert.AreEqual(1, result.Data.UserId);
            Assert.AreEqual(post2Title, result.Data.Title);
            Assert.AreEqual(post2Body, result.Data.Body);
        }

        [TestMethod]
        public void GetSpecificLeadingZerosTest()
        {
            RestRequest request = new RestRequest
            {
                Resource = "02"
            };

            RestClient.UseNewtonsoftJson();
            var result = RestClient.Get<Post>(request);

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.IsNotNull(result.Data);

            TestContext.WriteLine(result.ToString());

            Assert.AreEqual(2, result.Data.Id);
            Assert.AreEqual(1, result.Data.UserId);
            Assert.AreEqual(post2Title, result.Data.Title);
            Assert.AreEqual(post2Body, result.Data.Body);
        }
    }
}