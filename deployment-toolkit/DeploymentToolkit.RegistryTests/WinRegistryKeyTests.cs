using DeploymentToolkit.Registry;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.IO;

namespace DeploymentToolkit.RegistryTests
{
    [TestClass()]
    public class WinRegistryKeyTests
    {
        public Win32Registry Win32Registry = new Win32Registry();
        public Win64Registry Win64Registry = new Win64Registry();

        public const string Win32WriteLocation = @"HKEY_LOCAL_MACHINE\Software\Microsoft";
        public const string Win64WriteLocation = @"HKEY_LOCAL_MACHINE\Software\Microsoft";

        [TestMethod()]
        public void Win32WriteSubKeyTests()
        {
            using (var key = Win32Registry.OpenKey(Win32WriteLocation))
            {
                Assert.IsNotNull(key);

                var newKey = key.CreateSubKey("SubTest");
                Assert.IsNotNull(newKey);

                using (var subKey = Win32Registry.OpenKey(Path.Combine(Win32WriteLocation, "SubTest")))
                {
                    Assert.IsNotNull(newKey);
                }
            }
        }

        [TestMethod()]
        public void Win32WriteKeyTests()
        {
            using (var key = Win32Registry.CreateOrOpenKey(Path.Combine(Win32WriteLocation, "Test")))
            {
                Assert.IsNotNull(key);
            }
        }

        [TestMethod()]
        [TestCategory("SkipWhenLiveUnitTesting")]
        public void Win32DeleteTests()
        {
            using (var key = Win32Registry.OpenKey(Win32WriteLocation))
            {
                Assert.IsNotNull(key);

                key.DeleteSubKey("Test");
                key.DeleteSubKey("SubTest");
            }
        }

        [TestMethod()]
        public void Win64WriteSubKeyTests()
        {
            using (var key = Win64Registry.OpenKey(Win64WriteLocation))
            {
                Assert.IsNotNull(key);

                var newKey = key.CreateSubKey("SubTest");
                Assert.IsNotNull(newKey);

                using (var subKey = Win64Registry.OpenKey(Path.Combine(Win64WriteLocation, "SubTest")))
                {
                    Assert.IsNotNull(newKey);
                }
            }
        }

        [TestMethod()]
        public void Win64WriteKeyTests()
        {
            using (var key = Win64Registry.CreateOrOpenKey(Path.Combine(Win64WriteLocation, "Test")))
            {
                Assert.IsNotNull(key);
            }
        }

        [TestMethod()]
        public void Win64GetSubKeyTests()
        {
            using (var key = Win64Registry.OpenKey(Win64WriteLocation))
            {
                Assert.IsNotNull(key);

                var subKeys = key.GetSubKeys();
                
                foreach(var subKey in subKeys)
                {
                    Debug.WriteLine($"Path: {subKey.Key}");
                }
            }
        }

        [TestMethod()]
        [TestCategory("SkipWhenLiveUnitTesting")]
        public void Win64DeleteTests()
        {
            using (var key = Win64Registry.OpenKey(Win64WriteLocation))
            {
                Assert.IsNotNull(key);

                key.DeleteSubKey("Test");
                key.DeleteSubKey("SubTest");
            }
        }
    }
}
