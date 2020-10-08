using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;
using System.Collections.Generic;

namespace OloInterviewPart2
{
    [TestClass]
    public class GetTests : TestBase
    {
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
    }
}