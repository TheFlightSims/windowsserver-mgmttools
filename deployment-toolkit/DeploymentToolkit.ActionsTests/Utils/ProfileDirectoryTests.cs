using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace DeploymentToolkit.Actions.Utils.Tests
{
    [TestClass()]
    public class ProfileDirectoryTests
    {
        [TestMethod()]
        public void GetUserProfileUserTest()
        {
            var profiles = User.GetUserFolders(false, false);

            foreach (var profile in profiles)
            {
                Debug.WriteLine(profile);

                if (profile.Contains("Default"))
                    Assert.Fail("Default profile found");

                if (profile.Contains("Public"))
                    Assert.Fail("Public profile found");
            }
        }

        [TestMethod()]
        public void GetUserProfileDefaultTest()
        {
            var profiles = User.GetUserFolders(true, false);

            var defaultFound = false;

            foreach (var profile in profiles)
            {
                Debug.WriteLine(profile);

                if (profile.Contains("Default"))
                    defaultFound = true;

                if (profile.Contains("Public"))
                    Assert.Fail("Public profile found");
            }

            if (!defaultFound)
                Assert.Fail("Default profile not found");
        }

        [TestMethod()]
        public void GetUserProfilePublicTest()
        {
            var profiles = User.GetUserFolders(false, true);

            var publicFound = false;

            foreach (var profile in profiles)
            {
                Debug.WriteLine(profile);

                if (profile.Contains("Default"))
                    Assert.Fail("Default profile found");

                if (profile.Contains("Public"))
                    publicFound = true;
            }

            if (!publicFound)
                Assert.Fail("Public profile not found");
        }
    }
}
