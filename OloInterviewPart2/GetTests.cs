using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using System.Collections.Generic;

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
            var result = RestClient.Get<List<Post>>(GetRequest(string.Empty));

            LogGetResult(result);

            //TODO no good once i start posting and deleting things
            Assert.AreEqual(100, result.Data.Count, "Expected to return 100 results");

            Assert.AreEqual(1, result.Data[0].Id, "First post wasn't Id 1");
            Assert.AreEqual(1, result.Data[0].UserId, "First post wasnt user 1");
            Assert.AreEqual("sunt aut facere repellat provident occaecati excepturi optio reprehenderit", result.Data[0].Title);
            Assert.AreEqual("quia et suscipit\nsuscipit recusandae consequuntur expedita et cum\nreprehenderit molestiae ut ut quas totam\nnostrum rerum est autem sunt rem eveniet architecto",
                result.Data[0].Body);
        }

        [TestMethod]
        public void GetSpecificTest()
        {
            var result = RestClient.Get<Post>(GetRequest("2"));
            LogGetResult(result);

            Assert.AreEqual(2, result.Data.Id);
            Assert.AreEqual(1, result.Data.UserId);
            Assert.AreEqual(post2Title, result.Data.Title);
            Assert.AreEqual(post2Body, result.Data.Body);
        }

        [TestMethod]
        public void GetSpecificLeadingZerosTest()
        {
            var result = RestClient.Get<Post>(GetRequest("02"));
            LogGetResult(result);

            Assert.AreEqual(2, result.Data.Id);
            Assert.AreEqual(1, result.Data.UserId);
            Assert.AreEqual(post2Title, result.Data.Title);
            Assert.AreEqual(post2Body, result.Data.Body);
        }

        [TestMethod]
        public void GetByUserIdTest()
        {
            var request = GetRequest(string.Empty);
            request.AddParameter("userId", "2");
            var result = RestClient.Get<List<Post>>(request);

            LogGetResult(result);

            //TODO it would be better to test for an exact list but I cant control the data in this endpoint
            CollectionAssert.AllItemsAreInstancesOfType(result.Data, typeof(Post));
            Assert.AreEqual(10, result.Data.Count, "Expected to have 10 posts created by user 2");

            CollectionAssert.Contains(result.Data, 
                new Post {
                 Id = 11,
                 UserId= 2,
                Title = "et ea vero quia laudantium autem",
                Body = "delectus reiciendis molestiae occaecati non minima eveniet qui voluptatibus\naccusamus in eum beatae sit\nvel qui neque voluptates ut commodi qui incidunt\nut animi commodi" });

            Assert.IsTrue(result.Data.TrueForAll(post => post.UserId == 2), "Not all posts returned with user2 for Id");
        }

        [TestMethod]
        public void GetByTitleTest()
        {
            var title = "maxime id vitae nihil numquam";

            var request = GetRequest(string.Empty);
            //TODO I should create my own but that's not working
            request.AddParameter("title", title);
            var result = RestClient.Get<List<Post>>(request);

            LogGetResult(result);

            Assert.AreEqual(1, result.Data.Count);
            Assert.AreEqual(23, result.Data[0].Id);
            Assert.AreEqual(title, result.Data[0].Title);
            Assert.AreEqual("veritatis unde neque eligendi\nquae quod architecto quo neque vitae\nest illo sit tempora doloremque fugit quod\net et vel beatae sequi ullam sed tenetur perspiciatis",
                result.Data[0].Body);
        }
    }
}