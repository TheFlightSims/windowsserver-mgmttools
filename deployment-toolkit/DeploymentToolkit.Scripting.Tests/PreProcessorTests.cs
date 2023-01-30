using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;

namespace DeploymentToolkit.Scripting.Tests
{
    [TestClass()]
    public class PreProcessorTests
    {
        [TestMethod()]
        public void ProcessTest()
        {
            var conditions = new List<ExpectedConditon>()
            {
                new ExpectedConditon()
                {
                    Condition = "$Is64Bit$"
                },
                new ExpectedConditon()
                {
                    Condition = "$Is32Bit$"
                },
                new ExpectedConditon()
                {
                    Condition = "$DT_InstallPath$"
                },
                new ExpectedConditon()
                {
                    Condition = "$DT_FilesPath$"
                },
                new ExpectedConditon()
                {
                    Condition = "$DT_IsTaskSequence$"
                },

                new ExpectedConditon()
                {
                    Condition = @"('1' == '$Is64Bit$')"
                },

                // Wrongs
                new ExpectedConditon()
                {
                    Condition = "$DT_IdoNotExist$"
                },
            };

            foreach (var condition in conditions)
            {
                if (condition.ExpectedResult)
                    Assert.AreEqual(condition.Condition, PreProcessor.Process(condition.Condition));
                else
                    Assert.AreNotEqual(condition.Condition, PreProcessor.Process(condition.Condition));
            }
        }

        [TestMethod()]
        public void ProcessTestFunctions()
        {
            var conditions = new List<ExpectedConditon>()
            {
                new ExpectedConditon()
                {
                    Condition = @"('1' == '1')",
                    ExpectedResult = true
                },
                new ExpectedConditon()
                {
                    Condition = @"$DirectoryExists(C:\Windows)",
                    ExpectedResult = true
                },
                new ExpectedConditon()
                {
                    Condition = @"$FileExists(C:\Windows\explorer.exe)$"
                },
                new ExpectedConditon()
                {
                    Condition = @"$DirectoryExists(C:\Windows)$"
                },

                // Wrongs
                new ExpectedConditon()
                {
                    Condition = @"$DirectoryExists()$",
                },
                new ExpectedConditon()
                {
                    Condition = @"$DirectoryExists($",
                },
                new ExpectedConditon()
                {
                    Condition = @"$IdoNotExist(Test)$"
                },
            };

            foreach (var condition in conditions)
            {
                if (condition.ExpectedResult)
                    Assert.AreEqual(condition.Condition, PreProcessor.Process(condition.Condition));
                else
                    Assert.AreNotEqual(condition.Condition, PreProcessor.Process(condition.Condition));
            }
        }

        [TestMethod()]
        public void InlineVariableTest()
        {
            var conditions = new List<ExpectedConditon>()
            {
                new ExpectedConditon()
                {
                    Condition = "('$DirectoryExists($WinDir$)$' == '1')",
                    ExpectedResult = false
                },
            };

            foreach (var condition in conditions)
            {
                if (condition.ExpectedResult)
                    Assert.AreEqual(condition.Condition, PreProcessor.Process(condition.Condition));
                else
                    Assert.AreNotEqual(condition.Condition, PreProcessor.Process(condition.Condition));
            }
        }

        [TestMethod()]
        public void StringToIntTest()
        {
            var conditions = new List<ExpectedConditon>()
            {
                new ExpectedConditon()
                {
                    Condition = "('true' == '1')",
                    ExpectedResult = true
                },
                new ExpectedConditon()
                {
                    Condition = "('false' == '0')",
                    ExpectedResult = true
                }
            };

            foreach (var condition in conditions)
            {
                var preProcessed = PreProcessor.Process(condition.Condition);
                Assert.AreEqual(condition.ExpectedResult, Evaluation.Evaluate(preProcessed));
            }
        }

        [TestMethod()]
        public void EnvironmentVariablesTest()
        {
            var environmentVariables = Environment.GetEnvironmentVariables();

            foreach (DictionaryEntry variable in environmentVariables)
            {
                var name = (string)variable.Key;
                var value = (string)variable.Value;

                // Since () indicates a function with parameters we replace all () with [] in environmentvariables
                // So programfiles(x86) becomes programfiles[x86]
                if (name.Contains("("))
                    name = name.Replace("(", "[");
                if (name.Contains(")"))
                    name = name.Replace(")", "]");

                if (value.Contains("true"))
                    value = value.Replace("true", "1");
                if (value.Contains("fales"))
                    value = value.Replace("false", "0");

                var condition = $"('${name}$' == '{variable.Value}')";
                var expectedResult = $"('{value}' == '{value}')";

                var preProcessed = PreProcessor.Process(condition);
                Assert.AreEqual(preProcessed, expectedResult, $"Preprocessed '{preProcessed}' does not match expected result of '{expectedResult}'. Condition: {condition}");
            }
        }

        [TestMethod()]
        public void AddVariableTest()
        {
            var scripts = new List<ExpectedScript>()
            {
                new ExpectedScript()
                {
                    Name = "IsWindowsInstalled",
                    Script = @"function IsWindowsInstalled { return Test-Path C:\Windows }",
                    Result = true,
                    TestCondition = new ExpectedConditon()
                    {
                        Condition = @"('$IsWindowsInstalled$' == '1')",
                        ExpectedResult = true
                    }
                },
                new ExpectedScript()
                {
                    Name = "StringTest",
                    Script = @"function StringTest { return 'Test' }",
                    Result = true,
                    TestCondition = new ExpectedConditon()
                    {
                        Condition = @"('$StringTest$' == 'Test')",
                        ExpectedResult = true
                    }
                },

                // Same environments tests
                new ExpectedScript()
                {
                    Name = "IsEnvironment",
                    Script = "$environment = 'TEST'; function IsEnvironment { return 'Test' }",
                    Environment = "TEST",
                    Result = true,
                    TestCondition = new ExpectedConditon()
                    {
                        Condition = @"('$IsEnvironment$' == 'Test')",
                        ExpectedResult = true
                    }
                },
                new ExpectedScript()
                {
                    Name = "IsEnvironmentTest",
                    Script = @"function IsEnvironmentTest { return $environment }",
                    Environment = "TEST",
                    Result = true,
                    TestCondition = new ExpectedConditon()
                    {
                        Condition = @"('$IsEnvironmentTest$' == 'TEST')",
                        ExpectedResult = true
                    }
                },

                // Errors
                new ExpectedScript()
                {
                    Name = "Test",
                    Script = @"function WrongName { return Test-Path C:\Windows }",
                    Result = false,
                },

                // Double declaration
                new ExpectedScript()
                {
                    Name = "Test",
                    Script = @"function Test { return Test-Path C:\Windows }",
                    Result = true,
                    TestCondition = new ExpectedConditon()
                    {
                        Condition = @"('$Test$' == '1')",
                        ExpectedResult = true
                    }
                },
                new ExpectedScript()
                {
                    Name = "Test",
                    Script = @"function Test { return Test-Path C:\Windows }",
                    Result = false,
                }
            };

            foreach (var script in scripts)
            {
                var result = PreProcessor.AddVariable(script.Name, script.Script, script.Environment);
                Assert.AreEqual(script.Result, result);

                if (!result)
                    continue;

                var preProcessed = PreProcessor.Process(script.TestCondition.Condition);
                Assert.AreEqual(script.TestCondition.ExpectedResult, Evaluation.Evaluate(preProcessed));
            }
        }

        [TestMethod]
        public void TestPowerShellEnvironmentDipose()
        {
            Assert.IsTrue(PreProcessor.DisposePowerShellEnvironments());
        }
    }
}