using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;

namespace OloInterviewPart2
{
    [TestClass]
    public class PostTests : TestBase
    {
        [TestMethod]
        public void SimplePostTest()
        {
            var post = new Post
            {
                Title = "Test post",
                Body = "Test body",
                UserId = 1
            };
            RestRequest postRequest = new JsonRequest<Post, Post>(string.Empty, post);
            RestClient.UseNewtonsoftJson();

            var response = RestClient.Execute<Post>(postRequest, Method.POST);
            Assert.AreEqual(System.Net.HttpStatusCode.Created, response.StatusCode);
            Assert.IsTrue(response.Data.Id > 0, "Post not created with positive Id");

            var newPost = GetCreatedPost(response.Data.Id);
            Assert.AreEqual(post.Body, newPost.Body);
            Assert.AreEqual(post.Title, newPost.Title);
            Assert.AreEqual(post.UserId, newPost.UserId);
        }

        [TestMethod]
        public void IgnorePostIdTest()
        {
            var post = new Post
            {
                Title = "Test post",
                Body = "Test body",
                UserId = 1,
                Id = 10
            };
            RestRequest postRequest = new JsonRequest<Post, Post>(string.Empty, post);
            RestClient.UseNewtonsoftJson();

            var response = RestClient.Execute<Post>(postRequest, Method.POST);
            Assert.AreEqual(System.Net.HttpStatusCode.Created, response.StatusCode);
            Assert.IsTrue(response.Data.Id > 0, "Post not created with positive Id");
            Assert.AreNotEqual<int>(10, response.Data.Id);
        }

        [TestMethod]
        public void EmptyTitleRejectedTest()
        {
            var post = new Post
            {
                Title = string.Empty,
                Body = "Test body",
                UserId = 1
            };

            RestRequest postRequest = new JsonRequest<Post, Post>(string.Empty, post);
            RestClient.UseNewtonsoftJson();

            var response = RestClient.Execute<Post>(postRequest, Method.POST);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
            Assert.IsNull(response.Data);
        }
        //TODO Is there a limit on the number of characters a title or body can contain?

        [TestMethod]
        public void SpecialCharactersAcceptedTest()
        {
            var post = new Post
            {
                Title = "ﺧﺨﺩﺪﺫﺬﺭﺮﺯﺰﺱﺲ",
                Body = "ꏾꏿꐀꐁꐂꐃꐄꐅꐆꐇ ༫༬༭༮༯༰༱༲༳༴༸༺༻༼༽ཀ ᡊᡋᡌᡍᡎᡏᡐᡑᡒᡓᡔᡕᡖᡗᡘᡙᡚᡛ",
                UserId = 1
            };

            RestRequest postRequest = new JsonRequest<Post, Post>(string.Empty, post);
            RestClient.UseNewtonsoftJson();

            var response = RestClient.Execute<Post>(postRequest, Method.POST);
            var temp = JsonConvert.DeserializeObject<Post>(response.Content);
            Assert.AreEqual(System.Net.HttpStatusCode.Created, response.StatusCode);

            Assert.IsTrue(response.Data.Id > 0, "Post not created with positive Id");

            var newPost = GetCreatedPost(response.Data.Id);
            Assert.AreEqual(post.Body, newPost.Body);
            Assert.AreEqual(post.Title, newPost.Title);
            Assert.AreEqual(post.UserId, newPost.UserId);
        }

        [TestMethod]
        public void MissingUserRejectedTest()
        {
            var post = new Post
            {
                Title = "Title",
                Body = "Test body"
            };

            RestRequest postRequest = new JsonRequest<Post, Post>(string.Empty, post);
            RestClient.UseNewtonsoftJson();

            var response = RestClient.Execute<Post>(postRequest, Method.POST);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
            Assert.IsNull(response.Data);
        }

        [TestMethod]
        public void InvalidUserRejectedTest()
        {
            var post = new Post
            {
                Title = "Title",
                Body = "Test body",
                UserId = 1000
            };

            RestRequest postRequest = new JsonRequest<Post, Post>(string.Empty, post);
            RestClient.UseNewtonsoftJson();

            var response = RestClient.Execute<Post>(postRequest, Method.POST);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
            Assert.IsNull(response.Data);
        }


        private Post GetCreatedPost(int postId)
        {
            var getResponse = RestClient.Execute<Post>(GetRequest(postId.ToString()));
            Assert.AreEqual(System.Net.HttpStatusCode.OK, getResponse.StatusCode, $"Error retreiveing the post just created with Id {postId}");
            return getResponse.Data;
        }
    }
}