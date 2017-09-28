using AlertCenter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using System.Threading.Tasks;

namespace AlertCenterTests
{
    [TestClass]
    public class TestAggregater
    {
        private string TestsFailedString = "These Tests Failed:";
        private bool TestsPassed;

        [TestMethod]
        public async Task AggregateIntegrationTest()
        {
            var connectionProvider = new NpgsqlConnectionFactory(Config.TestConnectionString);

            TestsPassed = true;
            var methods = GetType().GetMethods();
            foreach (var method in methods)
                if (method.Name.IndexOf("Test") == 0)
                    await AggregateResult(method.Name.Substring(4), web);

            if (!TestsPassed)
                Assert.Fail(TestsFailedString);
        }

        private async Task AggregateResult(string testName, IWebRequester web)
        {
            bool result;
            try
            {
                var task = (Task<bool>)GetType().GetMethod("Test" + testName).Invoke(this, new object[] { web });
                await task.ConfigureAwait(false);
                result = task.Result;
            }
            catch
            {
                result = false;
            }
            if (!result)
            {
                TestsFailedString += (TestsPassed ? " " : ", ") + testName;
                TestsPassed = false;
            }
        }
    }
}
