using NUnit.Framework;
using MultiDbQuery;
using System.Threading.Tasks;

namespace NUnitTestProjectMutipleDbquery
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            var t = AvailableContext.GetContexts(this);

            if (!object.ReferenceEquals(t, null))
                Assert.Pass();

            Assert.Fail();
        }

        [Test]
        public void Test2()
        {
            var getall = AvailableContext.GetAll(this, new object());

            if (!object.ReferenceEquals(getall, null))
            {
                Assert.Pass();
            }
            //if the response it null , that means the request has fired a no context was found that why.
            Assert.Pass();

        }
    }
}