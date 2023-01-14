using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
namespace RemoteMsiManager.Tests
{
    [TestClass()]
    public class MsiProductTests
    {
        [TestMethod()]
        public void MsiProductTest()
        {
            string _identifyingNumber = "26A24AE4-039D-4CA4-87B4-2F86418065F0";
            string _name = "Java 8 Update 65 (64-bit)";
            string _version = "8.0.650.17";
            MsiProduct _MsiProduct = null;

            _MsiProduct = new MsiProduct(_identifyingNumber, _name, _version);

            Assert.AreEqual(_MsiProduct.IdentifyingNumber, _identifyingNumber);
            Assert.AreEqual(_MsiProduct.Name, _name);
            Assert.AreEqual(_MsiProduct.Version, _version);

            _MsiProduct = new MsiProduct(null, null, null);

            Assert.AreEqual(_MsiProduct.IdentifyingNumber, String.Empty);
            Assert.AreEqual(_MsiProduct.Name, String.Empty);
            Assert.AreEqual(_MsiProduct.Version, String.Empty);

            _MsiProduct = new MsiProduct(String.Empty, String.Empty, String.Empty);

            Assert.AreEqual(_MsiProduct.IdentifyingNumber, String.Empty);
            Assert.AreEqual(_MsiProduct.Name, String.Empty);
            Assert.AreEqual(_MsiProduct.Version, String.Empty);
        }

        [TestMethod]
        public void GetFormattedInstallDateTest()
        {
            string emptyDate = String.Empty;
            Assert.AreEqual(MsiProduct.GetFormattedInstallDate(emptyDate), String.Empty);

            string nullDate = null;
            Assert.AreEqual(MsiProduct.GetFormattedInstallDate(nullDate), String.Empty);

            string normalDate = "20160320";
            Assert.AreEqual(MsiProduct.GetFormattedInstallDate(normalDate), "20/03/2016");
        }

        [TestMethod()]
        public void GetConcatenatedVersionTest()
        {
            string _version = "8.0.650.17";
            Assert.AreEqual(MsiProduct.GetConcatenatedVersion(_version), "00008000000065000017");

            _version = "8.0.65013.17";
            Assert.AreEqual(MsiProduct.GetConcatenatedVersion(_version), "00008000006501300017");

            _version = "8.0.65013";
            Assert.AreEqual(MsiProduct.GetConcatenatedVersion(_version), "000080000065013");

            _version = "8 0 65013";
            Assert.AreEqual(MsiProduct.GetConcatenatedVersion(_version), String.Empty);

            _version = String.Empty;
            Assert.AreEqual(MsiProduct.GetConcatenatedVersion(_version), String.Empty);

            _version = null;
            Assert.AreEqual(MsiProduct.GetConcatenatedVersion(_version), String.Empty);
        }

        [TestMethod()]
        public void GetRegExpPatternTest()
        {
            string _wqlLikePattern = "26A24AE4-039D-4CA4-%-2F86418065F0";
            Assert.AreEqual(MsiProduct.GetRegExpPattern(_wqlLikePattern), @"26A24AE4-039D-4CA4-[ABCDEF\d\-]*-2F86418065F0");

            _wqlLikePattern = "26A24AE4-039D-4CA4-87_4-2F86418065F0";
            Assert.AreEqual(MsiProduct.GetRegExpPattern(_wqlLikePattern), @"26A24AE4-039D-4CA4-87[ABCDEF\d\-]4-2F86418065F0");
        }

        [TestMethod()]
        public void PatternMatchMsiCodeTest()
        {
            string _msiCode = "26A24AE4-039D-4CA4-87B4-2F86418065F0";
            string _pattern = "26A24AE4-039D-4CA4-87B4-2F86418065F0";
            Assert.IsTrue(MsiProduct.PatternMatchMsiCode(_msiCode, _pattern));

            _msiCode = "26A24AE4-039D-4CA4-87B4-2F86418065F0";
            _pattern = "26A24AE4-039D-4CA4-%-2F86418065F0";
            Assert.IsTrue(MsiProduct.PatternMatchMsiCode(_msiCode, _pattern));

            _msiCode = "26A24AE4-039D-4CA4-87B4-2F86418065F0";
            _pattern = "26A24AE4-039D-4CA4-87B4-2F864180%";
            Assert.IsTrue(MsiProduct.PatternMatchMsiCode(_msiCode, _pattern));

            _msiCode = "26A24AE4-039D-4CA4-87B4-2F86418065F0";
            _pattern = "%24AE4-039D-4CA4-87B4-2F86418065F0";
            Assert.IsTrue(MsiProduct.PatternMatchMsiCode(_msiCode, _pattern));

            _msiCode = "26A24AE4-039D-4CA4-87B4-2F86418065F0";
            _pattern = "26A24AE4-039D-4CA4-87_4-2F86418065F0";
            Assert.IsTrue(MsiProduct.PatternMatchMsiCode(_msiCode, _pattern));

            _msiCode = "26A24AE4-039D-4CA4-87B4-2F86418065F0";
            _pattern = "_6A24AE4-039D-4CA4-87B4-2F86418065F0";
            Assert.IsTrue(MsiProduct.PatternMatchMsiCode(_msiCode, _pattern));

            _msiCode = "26A24AE4-039D-4CA4-87B4-2F86418065F0";
            _pattern = "26A24AE4-039D-4CA4-87B4-2F86418065F_";
            Assert.IsTrue(MsiProduct.PatternMatchMsiCode(_msiCode, _pattern));

            _msiCode = "26A24AE4-039D-4CA4-87B4-2F86418065F0";
            _pattern = "26A24_E4-039D-4CA4-87B4-2F86_18065F0";
            Assert.IsTrue(MsiProduct.PatternMatchMsiCode(_msiCode, _pattern));

            _msiCode = "26A24AE4-039D-4CA4-87B4-2F86418065F0";
            _pattern = "26A__AE4-039D-%-87B4-2F86418065F0";
            Assert.IsTrue(MsiProduct.PatternMatchMsiCode(_msiCode, _pattern));

            _msiCode = "26A24AE4-039D-4CA4-87B4-2F86418065F0";
            _pattern = "16A__AE4-039D-%-87B4-2F86418065F0";
            Assert.IsFalse(MsiProduct.PatternMatchMsiCode(_msiCode, _pattern));

            _msiCode = "26A24AE4-039D-4CA4-87B4-2F86418065F0";
            _pattern = "26A24AE4-039D-4CA4-77B4-2F86418065F0";
            Assert.IsFalse(MsiProduct.PatternMatchMsiCode(_msiCode, _pattern));

            _msiCode = "26A24AE4-039D-4CA4-87B4-2F86418065F0";
            _pattern = "26A24AE4-039D-4CA4-7%-2F86418065F0";
            Assert.IsFalse(MsiProduct.PatternMatchMsiCode(_msiCode, _pattern));

            _msiCode = "26A24AE4-039D-4CA4-87B4-2F86418065F0";
            _pattern = "%5A24AE4-039D-4CA4-77B4-2F86418065F0";
            Assert.IsFalse(MsiProduct.PatternMatchMsiCode(_msiCode, _pattern));

            _msiCode = "26A24AE4-039D-4CA4-87B4-2F86418065F0";
            _pattern = "16A24AE4-039D-4CA4-77B4-2F86418065F%";
            Assert.IsFalse(MsiProduct.PatternMatchMsiCode(_msiCode, _pattern));

            _msiCode = "26A24AE4-039D-4CA4-87B4-2F86418065F0";
            _pattern = "26A24AE4-0_8D-4CA4-77B4-2F86418065F0";
            Assert.IsFalse(MsiProduct.PatternMatchMsiCode(_msiCode, _pattern));

            _msiCode = "26A24AE4-039D-4CA4-87B4-2F86418065F0";
            _pattern = "25A24AE4-0__D-4CA4-77B4-2F86418065F0";
            Assert.IsFalse(MsiProduct.PatternMatchMsiCode(_msiCode, _pattern));

        }

        [TestMethod()]
        public void RemoveUnvantedCharactersTest()
        {
            string _goodString = "ABcdEF-012-23232;456879%_";

            Assert.AreEqual(MsiProduct.RemoveUnvantedCharacters(String.Empty), String.Empty);
            Assert.AreEqual(MsiProduct.RemoveUnvantedCharacters(null), String.Empty);
            Assert.AreEqual(MsiProduct.RemoveUnvantedCharacters(_goodString), _goodString);
            Assert.AreEqual("BeaeDadCE", MsiProduct.RemoveUnvantedCharacters("Bonjour, je m'appel David COURTEL"));
            Assert.AreEqual("ce-ae%de_ade;eeaedCaace", MsiProduct.RemoveUnvantedCharacters("Voici un test-unitaire %de_la méthode; RemoveUnwantedCharacters"));
            Assert.AreEqual("aeBcceae%de_ade;eeaedCaace234-4242", MsiProduct.RemoveUnvantedCharacters("aeBc Voici un test unitaire %de_la méthode; RemoveUnwantedCharacters234-4242"));
        }

        [TestMethod()]
        public void SplitMsiProductCodesTest()
        {
            List<string> _actual = new List<string>();

            _actual = MsiProduct.SplitMsiProductCodes("26A24AE4-039D-4CA4-87B4-2F86418065F0");
            Assert.IsTrue(_actual.Count == 1);
            Assert.IsTrue(_actual.Contains("26A24AE4-039D-4CA4-87B4-2F86418065F0"));

            _actual.Clear();
            _actual = MsiProduct.SplitMsiProductCodes("26A24AE4-039D-4CA4-87B4-2F86418065F0;4A2E75DE-133F-4239-B6A4-90658ECFC22E");
            Assert.IsTrue(_actual.Count == 2);
            Assert.IsTrue(_actual[0] == "26A24AE4-039D-4CA4-87B4-2F86418065F0");
            Assert.IsTrue(_actual[1] == "4A2E75DE-133F-4239-B6A4-90658ECFC22E");

            _actual.Clear();
            _actual = MsiProduct.SplitMsiProductCodes("26A24AE4-039D-4CA4-87B4-2F86418065F0;4A2E75DE-133F-4239-B6A4-90658ECFC22E;;7BF61FA9-BDFB-4563-98AD-FCB0DA28CCC7");
            Assert.IsTrue(_actual.Count == 3);
            Assert.IsTrue(_actual[0] == "26A24AE4-039D-4CA4-87B4-2F86418065F0");
            Assert.IsTrue(_actual[1] == "4A2E75DE-133F-4239-B6A4-90658ECFC22E");
            Assert.IsTrue(_actual[2] == "7BF61FA9-BDFB-4563-98AD-FCB0DA28CCC7");

            _actual.Clear();
            _actual = MsiProduct.SplitMsiProductCodes("26A24AE4-039D-4CA4-87B4-2F86418065F0;4A2E75DE-133F-4239-B6A4-90658ECFC22E;;7BF61FA9-BDFB-4563-98AD-FCB0DA28CCC7;;;;");
            Assert.IsTrue(_actual.Count == 3);
            Assert.IsTrue(_actual[0] == "26A24AE4-039D-4CA4-87B4-2F86418065F0");
            Assert.IsTrue(_actual[1] == "4A2E75DE-133F-4239-B6A4-90658ECFC22E");
            Assert.IsTrue(_actual[2] == "7BF61FA9-BDFB-4563-98AD-FCB0DA28CCC7");

            _actual.Clear();
            _actual = MsiProduct.SplitMsiProductCodes("26A24AE4-039D-4CA4-8%B4-2F86418065F0;4A2__5DE-133F-4239-B6A4-90658ECFC22E;;%FA9-BDFB-4563-98AD-FCB0DA28____");
            Assert.IsTrue(_actual.Count == 3);
            Assert.IsTrue(_actual[0] == "26A24AE4-039D-4CA4-8%B4-2F86418065F0");
            Assert.IsTrue(_actual[1] == "4A2__5DE-133F-4239-B6A4-90658ECFC22E");
            Assert.IsTrue(_actual[2] == "%FA9-BDFB-4563-98AD-FCB0DA28____");
        }

        [TestMethod]
        public void GetErrorMessageTest()
        {
            string expected = "The action completed successfully.";
            string actual = MsiProduct.GetErrorMessage(0);

            Assert.AreEqual(expected, actual);

            expected = String.Empty;
            actual = MsiProduct.GetErrorMessage(655535);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void IsSuccess()
        {
            Assert.IsTrue(MsiProduct.IsSuccess(0));
            Assert.IsTrue(MsiProduct.IsSuccess(1641));
            Assert.IsTrue(MsiProduct.IsSuccess(3010));
            Assert.IsFalse(MsiProduct.IsSuccess(1602));
        }

        [TestMethod]
        public void IsRebootNeeded()
        {
            Assert.IsTrue(MsiProduct.IsRebootNeeded(1641));
            Assert.IsTrue(MsiProduct.IsRebootNeeded(3010));
            Assert.IsFalse(MsiProduct.IsRebootNeeded(0));
        }
    }
}
