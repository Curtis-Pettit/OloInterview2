using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;
using System;
using System.Collections.Generic;
using System.Text;

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

            RestRequest getRequest = new RestRequest
            {
                Resource = response.Data.Id.ToString()
            };
            var getResponse = RestClient.Execute<Post>(getRequest, Method.GET);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(post.Body, getResponse.Data.Body);
            Assert.AreEqual(post.Title, getResponse.Data.Title);
            Assert.AreEqual(post.UserId, getResponse.Data.UserId);
        }    
    }
}