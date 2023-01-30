using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace DeploymentToolkit.Actions.Utils.Tests
{
    [TestClass()]
    public class RegistryTests
    {
        [TestMethod()]
        public void GetUserRegistryUserTest()
        {
            var users = User.GetUserRegistry(false, false);

            foreach (var user in users)
            {
                Debug.WriteLine(user);

                if (user.Contains(".DEFAULT"))
                    Assert.Fail($".DEFAULT profile found ({user})");

                if (user.Contains("_Classes"))
                    Assert.Fail($"_Classes found ({user})");
            }
        }

        [TestMethod()]
        public void GetUserRegistryDefaultTest()
        {
            var users = User.GetUserRegistry(true, false);

            var defaultFound = false;

            foreach (var user in users)
            {
                Debug.WriteLine(user);

                if (user.Contains(".DEFAULT"))
                    defaultFound = true;

                if (user.Contains("_Classes"))
                    Assert.Fail($"_Classes found ({user})");
            }

            if (!defaultFound)
                Assert.Fail(".DEFAULT profile not found");
        }

        [TestMethod()]
        public void GetUserRegistrySpecialTest()
        {
            var users = User.GetUserRegistry(false, true);

            var specialFound = false;

            foreach (var user in users)
            {
                Debug.WriteLine(user);

                if (user.Length == 19 && user.StartsWith(@"HKEY_USERS\S-"))
                    specialFound = true;

                if (user.Contains("_Classes"))
                    Assert.Fail($"_Classes found ({user})");
            }

            if (!specialFound)
                Assert.Fail("Failed to find special profiles");
        }
    }
}