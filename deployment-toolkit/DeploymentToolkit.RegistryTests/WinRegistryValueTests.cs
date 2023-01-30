using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.ComponentModel;
using System.Diagnostics;

namespace DeploymentToolkit.Registry.Tests
{
    [TestClass()]
    public class WinRegistryValueTests
    {
        public Win32Registry Win32Registry = new Win32Registry();
        public Win64Registry Win64Registry = new Win64Registry();

        public const string TypeExceptionLocation = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control";
        public const string HiveExceptionLocation = @"HKEY_LOCAL_ROBOT\Microsoft";
        public const string Win32ReadLocation = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion";
        public const string Win64ReadLocation = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion";
        public const string Win64WriteLocation = @"HKEY_CURRENT_USER\Software\Microsoft";

        [TestMethod()]
        public void TypeExceptionTest()
        {
            using (var key = Win32Registry.OpenKey(TypeExceptionLocation))
            {
                Assert.IsNotNull(key);

                Assert.ThrowsException<ArgumentOutOfRangeException>(delegate ()
                {
                    key.GetValue<string>("EarlyStartServices", Microsoft.Win32.RegistryValueKind.MultiString);
                });
            }
        }

        [TestMethod()]
        public void HiveExceptionTest()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(delegate ()
            {
                Win32Registry.OpenKey(HiveExceptionLocation);
            });
        }

        [TestMethod()]
        public void Win32ReadTests()
        {
            using (var key = Win32Registry.OpenKey(Win32ReadLocation))
            {
                Assert.IsNotNull(key);

                var name = key.GetValue<string>("ProductName", Microsoft.Win32.RegistryValueKind.String);
                Assert.IsNotNull(name);

                var version = key.GetValue<int>("CurrentMajorVersionNumber", Microsoft.Win32.RegistryValueKind.DWord);
                Assert.AreNotSame(version, 0);

                // QWords do not exists unter 32 bit so this key dosen't exist in the 32 bit space
                Assert.ThrowsException<Win32Exception>(delegate()
                {
                    key.GetValue<long>("InstallTime", Microsoft.Win32.RegistryValueKind.QWord);
                });

                Debug.WriteLine(name);
                Debug.WriteLine(version);
            }
        }

        [TestMethod()]
        public void Win64ReadTests()
        {
            using (var key = Win64Registry.OpenKey(Win64ReadLocation))
            {
                Assert.IsNotNull(key);

                var name = key.GetValue<string>("ProductName", Microsoft.Win32.RegistryValueKind.String);
                Assert.IsNotNull(name);

                var version = key.GetValue<int>("CurrentMajorVersionNumber", Microsoft.Win32.RegistryValueKind.DWord);
                Assert.AreNotSame(version, 0);

                var installTime = key.GetValue<long>("InstallTime", Microsoft.Win32.RegistryValueKind.QWord);
                Assert.AreNotSame(installTime, 0);

                Debug.WriteLine(name);
                Debug.WriteLine(version);
                Debug.WriteLine(installTime);
            }
        }

        [TestMethod()]
        public void Win64WriteTests()
        {
            using (var key = Win32Registry.OpenKey(Win64WriteLocation, true))
            {
                Assert.IsNotNull(key);

                var stringValue = "Test";
                var intValue = 1;
                var longValue = 2;

                key.SetValue("StringTest", stringValue, Microsoft.Win32.RegistryValueKind.String);
                key.SetValue("IntTest", intValue, Microsoft.Win32.RegistryValueKind.DWord);
                key.SetValue("LongTest", longValue, Microsoft.Win32.RegistryValueKind.QWord);

                var stringTest = key.GetValue<string>("StringTest", Microsoft.Win32.RegistryValueKind.String);
                var intTest = key.GetValue<int>("IntTest", Microsoft.Win32.RegistryValueKind.DWord);
                var longTest = key.GetValue<long>("LongTest", Microsoft.Win32.RegistryValueKind.QWord);

                Assert.AreEqual(stringValue, stringTest);
                Assert.AreEqual(intValue, intTest);
                Assert.AreEqual(longValue, longTest);

                Debug.WriteLine(stringTest);
                Debug.WriteLine(intTest);
                Debug.WriteLine(longTest);
            }
        }

        [TestMethod()]
        [TestCategory("SkipWhenLiveUnitTesting")]
        public void Win64DeleteTests()
        {
            using (var key = Win32Registry.OpenKey(Win64WriteLocation, true))
            {
                key.DeleteValue("StringTest");
                key.DeleteValue("IntTest");
                key.DeleteValue("LongTest");
            }
        }
    }
}