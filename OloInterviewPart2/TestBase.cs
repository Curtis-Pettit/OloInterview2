using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

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

    }
}
