using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace DeploymentToolkit.Scripting.Tests
{
    [TestClass()]
    public class RegistryTests
    {
        [TestInitialize()]
        public void PrepareEnvironment()
        {
            Actions.RegistryActions.SetValue(Actions.RegistryActions.Architecture.Win64, @"HKEY_CURRENT_USER\SOFTWARE\", "DT_TEST", "TEST", Microsoft.Win32.RegistryValueKind.String);
            Actions.RegistryActions.SetValue(Actions.RegistryActions.Architecture.Win64, @"HKEY_CURRENT_USER\SOFTWARE\", "DT_TEST_DWORD", 1337, Microsoft.Win32.RegistryValueKind.DWord);
        }

        [TestMethod()]
        public void TestArchitecture()
        {
            var conditions = new List<ExpectedConditon>()
            {
                new ExpectedConditon()
                {
                    Condition = "('$RegHasKey(0, HKEY_LOCAL_MACHINE, SOFTWARE)$' == '1')",
                    ExpectedResult = true
                },
                new ExpectedConditon()
                {
                    Condition = "('$RegHasKey(Win32, HKEY_LOCAL_MACHINE, SOFTWARE)$' == '1')",
                    ExpectedResult = true
                },
                new ExpectedConditon()
                {
                    Condition = "('$RegHasKey(1, HKEY_LOCAL_MACHINE, SOFTWARE)$' == '1')",
                    ExpectedResult = true
                },
                new ExpectedConditon()
                {
                    Condition = "('$RegHasKey(Win64, HKEY_LOCAL_MACHINE, SOFTWARE)$' == '1')",
                    ExpectedResult = true
                },

                new ExpectedConditon()
                {
                    Condition = "('$RegHasKey(-1, HKEY_LOCAL_MACHINE, SOFTWARE)$' == '1')",
                    ExpectedResult = false
                },
                new ExpectedConditon()
                {
                    Condition = "('$RegHasKey(3, HKEY_LOCAL_MACHINE, SOFTWARE)$' == '1')",
                    ExpectedResult = false
                },
                new ExpectedConditon()
                {
                    Condition = "('$RegHasKey(9999999, HKEY_LOCAL_MACHINE, SOFTWARE)$' == '1')",
                    ExpectedResult = false
                },
                new ExpectedConditon()
                {
                    Condition = "('$RegHasKey(Win96, HKEY_LOCAL_MACHINE, SOFTWARE)$' == '1')",
                    ExpectedResult = false
                },
            };

            foreach (var condition in conditions)
            {
                var preProcessed = PreProcessor.Process(condition.Condition);
                var result = Evaluation.Evaluate(preProcessed);
                Assert.AreEqual(condition.ExpectedResult, result, $"Ran '{condition.Condition}' and expected '{condition.ExpectedResult}' but got '{result}'. PreProcessed: '{preProcessed}'");
            }
        }

        [TestMethod()]
        public void RegHasKey()
        {
            var conditions = new List<ExpectedConditon>()
            {
                new ExpectedConditon()
                {
                    Condition = "('$RegHasKey(0, HKEY_LOCAL_MACHINE, SOFTWARE)$' == '1')",
                    ExpectedResult = true
                },

                new ExpectedConditon()
                {
                    Condition = "('$RegHasKey(0, SOFTWARE)$' == '1')",
                    ExpectedResult = false
                },
                new ExpectedConditon()
                {
                    Condition = "('$RegHasKey(0, , SOFTWARE)$' == '1')",
                    ExpectedResult = false
                },
                new ExpectedConditon()
                {
                    Condition = "('$RegHasKey(0, HKEY_LOCAL_MACHINE, )$' == '1')",
                    ExpectedResult = false
                },
                new ExpectedConditon()
                {
                    Condition = "('$RegHasKey(0, , )$' == '1')",
                    ExpectedResult = false
                },
            };

            foreach(var condition in conditions)
            {
                var preProcessed = PreProcessor.Process(condition.Condition);
                var result = Evaluation.Evaluate(preProcessed);
                Assert.AreEqual(condition.ExpectedResult, result, $"Ran '{condition.Condition}' and expected '{condition.ExpectedResult}' but got '{result}'. PreProcessed: '{preProcessed}'");
            }
        }

        [TestMethod()]
        public void RegCreateKey()
        {
            var conditions = new List<ExpectedConditon>()
            {
                new ExpectedConditon()
                {
                    Condition = @"('$RegCreateKey(0, HKEY_CURRENT_USER\SOFTWARE, DT_TEST)$' == '1')",
                    ExpectedResult = true
                },

                new ExpectedConditon()
                {
                    Condition = @"('$RegCreateKey(0, HKEY_CURRENT_USER\SOFTWARE)$' == '1')",
                    ExpectedResult = false
                },
                new ExpectedConditon()
                {
                    Condition = @"('$RegCreateKey(0, , DT_TEST)$' == '1')",
                    ExpectedResult = false
                },
                new ExpectedConditon()
                {
                    Condition = @"('$RegCreateKey(0, HKEY_CURRENT_USER\SOFTWARE, )$' == '1')",
                    ExpectedResult = false
                },
                new ExpectedConditon()
                {
                    Condition = @"('$RegCreateKey(0, , )$' == '1')",
                    ExpectedResult = false
                }
            };

            foreach (var condition in conditions)
            {
                var preProcessed = PreProcessor.Process(condition.Condition);
                var result = Evaluation.Evaluate(preProcessed);
                Assert.AreEqual(condition.ExpectedResult, result, $"Ran '{condition.Condition}' and expected '{condition.ExpectedResult}' but got '{result}'. PreProcessed: '{preProcessed}'");
            }
        }

        [TestMethod()]
        public void RegHasValue()
        {
            var conditions = new List<ExpectedConditon>()
            {
                new ExpectedConditon()
                {
                    Condition = @"('$RegHasValue(0, HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer, UserSignedIn)$' == '1')",
                    ExpectedResult = true
                },

                new ExpectedConditon()
                {
                    Condition = @"('$RegHasValue(0, HKEY_CURRENT_USER\SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion)$' == '1')",
                    ExpectedResult = false
                },
                new ExpectedConditon()
                {
                    Condition = @"('$RegHasValue(0, , ProgramFilesDir)$' == '1')",
                    ExpectedResult = false
                },
                new ExpectedConditon()
                {
                    Condition = @"('$RegHasValue(0, HKEY_CURRENT_USER\SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion, )$' == '1')",
                    ExpectedResult = false
                },
                new ExpectedConditon()
                {
                    Condition = @"('$RegHasValue(0, , )$' == '1')",
                    ExpectedResult = false
                },
            };

            foreach (var condition in conditions)
            {
                var preProcessed = PreProcessor.Process(condition.Condition);
                var result = Evaluation.Evaluate(preProcessed);
                Assert.AreEqual(condition.ExpectedResult, result, $"Ran '{condition.Condition}' and expected '{condition.ExpectedResult}' but got '{result}'. PreProcessed: '{preProcessed}'");
            }
        }

        [TestMethod()]
        public void RegSetValue()
        {
            var conditions = new List<ExpectedConditon>()
            {
                new ExpectedConditon()
                {
                    Condition = @"('$RegSetValue(0, HKEY_CURRENT_USER\SOFTWARE, DT_TEST, TEST, 1)$' == '1')",
                    ExpectedResult = true,
                },
                new ExpectedConditon()
                {
                    Condition = @"('$RegSetValue(0, HKEY_CURRENT_USER\SOFTWARE, DT_TEST_EMPTY, , 1)$' == '1')",
                    ExpectedResult = true,
                },
                new ExpectedConditon()
                {
                    Condition = @"('$RegSetValue(0, HKEY_CURRENT_USER\SOFTWARE, DT_TEST_DWORD, 1337, 4)$' == '1')",
                    ExpectedResult = true,
                },

                new ExpectedConditon()
                {
                    Condition = @"('$RegSetValue(0, HKEY_CURRENT_USER\SOFTWARE, DT_TEST, TEST)$' == '1')",
                    ExpectedResult = false,
                },
                new ExpectedConditon()
                {
                    Condition = @"('$RegSetValue(0, HKEY_CURRENT_USER\SOFTWARE, DT_TEST, TEST, )$' == '1')",
                    ExpectedResult = false,
                },
                new ExpectedConditon()
                {
                    Condition = @"('$RegSetValue(0, HKEY_CURRENT_USER\SOFTWARE, , TEST, 1)$' == '1')",
                    ExpectedResult = false,
                },
                new ExpectedConditon()
                {
                    Condition = @"('$RegSetValue(0, , DT_TEST, TEST, 1)$' == '1')",
                    ExpectedResult = false,
                },
                new ExpectedConditon()
                {
                    Condition = @"('$RegSetValue(, HKEY_CURRENT_USER\SOFTWARE, DT_TEST, TEST, 1)$' == '1')",
                    ExpectedResult = false,
                },
                new ExpectedConditon()
                {
                    Condition = @"('$RegSetValue(0, , , , )$' == '1')",
                    ExpectedResult = false,
                },

                new ExpectedConditon()
                {
                    Condition = @"('$RegSetValue(0, HKEY_CURRENT_USER\SOFTWARE, DT_TEST, TEST, 4)$' == '1')",
                    ExpectedResult = false,
                },
            };

            foreach (var condition in conditions)
            {
                var preProcessed = PreProcessor.Process(condition.Condition);
                var result = Evaluation.Evaluate(preProcessed);
                Assert.AreEqual(condition.ExpectedResult, result, $"Ran '{condition.Condition}' and expected '{condition.ExpectedResult}' but got '{result}'. PreProcessed: '{preProcessed}'");
            }
        }

        [TestMethod()]
        public void RegGetValue()
        {
            var conditions = new List<ExpectedConditon>()
            {
                new ExpectedConditon()
                {
                    Condition = @"('$RegGetValue(0, HKEY_CURRENT_USER\SOFTWARE, DT_TEST)$' == 'TEST')",
                    ExpectedResult = true
                },
                new ExpectedConditon()
                {
                    Condition = @"('$RegGetValue(0, HKEY_CURRENT_USER\SOFTWARE, DT_TEST_EMPTY)$' == '')",
                    ExpectedResult = true
                },
                new ExpectedConditon()
                {
                    Condition = @"('$RegGetValue(0, HKEY_CURRENT_USER\SOFTWARE, DT_TEST_DWORD)$' == '1337')",
                    ExpectedResult = true
                },

                new ExpectedConditon()
                {
                    Condition = @"('$RegGetValue(0, HKEY_CURRENT_USER\SOFTWARE)$' == 'TEST')",
                    ExpectedResult = false
                },
                new ExpectedConditon()
                {
                    Condition = @"('$RegGetValue(0, HKEY_CURRENT_USER\SOFTWARE, )$' == 'TEST')",
                    ExpectedResult = false
                },
                new ExpectedConditon()
                {
                    Condition = @"('$RegGetValue(0, , DT_TEST)$' == 'TEST')",
                    ExpectedResult = false
                },
                new ExpectedConditon()
                {
                    Condition = @"('$RegGetValue(0, , )$' == 'TEST')",
                    ExpectedResult = false
                }
            };

            foreach (var condition in conditions)
            {
                var preProcessed = PreProcessor.Process(condition.Condition);
                var result = Evaluation.Evaluate(preProcessed);
                Assert.AreEqual(condition.ExpectedResult, result, $"Ran '{condition.Condition}' and expected '{condition.ExpectedResult}' but got '{result}'. PreProcessed: '{preProcessed}'");
            }
        }

        [TestMethod()]
        public void RegDeleteValue()
        {
            var conditions = new List<ExpectedConditon>()
            {
                new ExpectedConditon()
                {
                    Condition = @"('$RegDeleteValue(0, HKEY_CURRENT_USER\SOFTWARE, DT_TEST)$' == '1')",
                    ExpectedResult = true
                },
                new ExpectedConditon()
                {
                    Condition = @"('$RegDeleteValue(0, HKEY_CURRENT_USER\SOFTWARE, DT_TEST_EMPTY)$' == '1')",
                    ExpectedResult = true
                },
                new ExpectedConditon()
                {
                    Condition = @"('$RegDeleteValue(0, HKEY_CURRENT_USER\SOFTWARE, DT_TEST_DWORD)$' == '1')",
                    ExpectedResult = true
                },

                new ExpectedConditon()
                {
                    Condition = @"('$RegDeleteValue(0, HKEY_CURRENT_USER\SOFTWARE)$' == '1')",
                    ExpectedResult = false
                },
                new ExpectedConditon()
                {
                    Condition = @"('$RegDeleteValue(0, HKEY_CURRENT_USER\SOFTWARE, )$' == '1')",
                    ExpectedResult = false
                },
                new ExpectedConditon()
                {
                    Condition = @"('$RegDeleteValue(0, , DT_TEST)$' == '1')",
                    ExpectedResult = false
                },
                new ExpectedConditon()
                {
                    Condition = @"('$RegDeleteValue(0, , )$' == '1')",
                    ExpectedResult = false
                }
            };

            foreach (var condition in conditions)
            {
                var preProcessed = PreProcessor.Process(condition.Condition);
                var result = Evaluation.Evaluate(preProcessed);
                Assert.AreEqual(condition.ExpectedResult, result, $"Ran '{condition.Condition}' and expected '{condition.ExpectedResult}' but got '{result}'. PreProcessed: '{preProcessed}'");
            }
        }

        [TestMethod()]
        public void RegDeleteKey()
        {
            var conditions = new List<ExpectedConditon>()
            {
                new ExpectedConditon()
                {
                    Condition = @"('$RegDeleteKey(0, HKEY_CURRENT_USER\SOFTWARE, DT_TEST)$' == '1')",
                    ExpectedResult = true
                },

                new ExpectedConditon()
                {
                    Condition = @"('$RegDeleteKey(0, HKEY_CURRENT_USER\SOFTWARE)$' == '1')",
                    ExpectedResult = false
                },
                new ExpectedConditon()
                {
                    Condition = @"('$RegDeleteKey(0, , DT_TEST)$' == '1')",
                    ExpectedResult = false
                },
                new ExpectedConditon()
                {
                    Condition = @"('$RegDeleteKey(0, HKEY_CURRENT_USER\SOFTWARE, )$' == '1')",
                    ExpectedResult = false
                },
                new ExpectedConditon()
                {
                    Condition = @"('$RegDeleteKey(0, , )$' == '1')",
                    ExpectedResult = false
                }
            };

            foreach (var condition in conditions)
            {
                var preProcessed = PreProcessor.Process(condition.Condition);
                var result = Evaluation.Evaluate(preProcessed);
                Assert.AreEqual(condition.ExpectedResult, result, $"Ran '{condition.Condition}' and expected '{condition.ExpectedResult}' but got '{result}'. PreProcessed: '{preProcessed}'");
            }
        }
    }
}
