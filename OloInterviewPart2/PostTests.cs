using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;
using System;
using System.Net;

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

            RestRequest postRequest = InitializePostRequest(post);

            var response = RestClient.Execute<Post>(postRequest, Method.POST);
            LogPostResult(response);

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

            RestRequest postRequest = InitializePostRequest(post);

            var result = RestClient.Execute<Post>(postRequest, Method.POST);

            LogPostResult(result);

            Assert.IsTrue(result.Data.Id > 0, "Post not created with positive Id");
            Assert.AreNotEqual(10, result.Data.Id);
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow("  ")]
        [DataRow(null)]
        public void EmptyTitleRejectedTest(string title)
        {
            var post = new Post
            {
                Title = title,
                Body = "Test body",
                UserId = 1
            };

            RestRequest postRequest = InitializePostRequest(post);

            var result = RestClient.Execute<Post>(postRequest, Method.POST);
            VerifyRejectedResult(result);
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

            RestRequest postRequest = InitializePostRequest(post);

            var result = RestClient.Execute<Post>(postRequest, Method.POST);
            LogPostResult(result);

            Assert.IsTrue(result.Data.Id > 0, "Post not created with positive Id");

            var newPost = GetCreatedPost(result.Data.Id);
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

            RestRequest postRequest = InitializePostRequest(post);

            var result = RestClient.Execute<Post>(postRequest, Method.POST);
            VerifyRejectedResult(result);
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

            RestRequest postRequest = InitializePostRequest(post);

            var result = RestClient.Execute<Post>(postRequest, Method.POST);
            VerifyRejectedResult(result);
        }

        private void VerifyRejectedResult(IRestResponse<Post> result)
        {

            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.IsNull(result.Data);
        }

        private void LogPostResult(IRestResponse<Post> result)
        {
            Assert.AreEqual(HttpStatusCode.Created, result.StatusCode);
            Assert.IsNotNull(result.Data, $"Failed to deserailize. Content was: {result.Content}");
            TestContext.WriteLine($"Post returned: {Environment.NewLine}" + result.Data);
        }

        private Post GetCreatedPost(int postId)
        {
            var getResponse = RestClient.Execute<Post>(GetRequest(postId.ToString()));
            Assert.AreEqual(System.Net.HttpStatusCode.OK, getResponse.StatusCode, $"Error retreiveing the post just created with Id {postId}");
            return getResponse.Data;
        }

        private JsonRequest<Post, Post> InitializePostRequest(Post post)
        {
            JsonRequest<Post, Post> postRequest = new JsonRequest<Post, Post>(string.Empty, post);
            RestClient.UseNewtonsoftJson();
            return postRequest;
        }
    }
}