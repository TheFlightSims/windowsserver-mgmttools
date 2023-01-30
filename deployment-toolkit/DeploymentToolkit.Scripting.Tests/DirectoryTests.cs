using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace DeploymentToolkit.Scripting.Tests
{
    [TestClass()]
    public class DirectoryTests
    {
        [TestMethod()]
        public void DirectoryExists()
        {
            var conditions = new List<ExpectedConditon>()
            {
                new ExpectedConditon()
                {
                    Condition = @"('$DirectoryExists($windir$)$' == '1')",
                    ExpectedResult = true
                },
                new ExpectedConditon()
                {
                    Condition = @"('$DirectoryExists(C:\InvalidFolder)$' == '1')",
                    ExpectedResult = false
                },

                new ExpectedConditon()
                {
                    Condition = @"('$DirectoryExists()$' == '1')",
                    ExpectedResult = false
                }
            };

            foreach (var condition in conditions)
            {
                var preProcessed = PreProcessor.Process(condition.Condition);
                var evaluated = Evaluation.Evaluate(preProcessed);
                Assert.AreEqual(condition.ExpectedResult, evaluated, $"Expected '{condition.ExpectedResult}' but got '{evaluated}' ({condition.Condition} -> {preProcessed})");
            }
        }

        [TestMethod()]
        public void DirectoryCreate()
        {
            var conditions = new List<ExpectedConditon>()
            {
                new ExpectedConditon()
                {
                    Condition = @"('$DirectoryCreate(C:\Temp\DT_TEST)$' == '1')",
                    ExpectedResult = true
                },

                new ExpectedConditon()
                {
                    Condition = @"('$DirectoryCreate()$' == '1')",
                    ExpectedResult = false
                }
            };

            foreach (var condition in conditions)
            {
                var preProcessed = PreProcessor.Process(condition.Condition);
                var evaluated = Evaluation.Evaluate(preProcessed);
                Assert.AreEqual(condition.ExpectedResult, evaluated, $"Expected '{condition.ExpectedResult}' but got '{evaluated}' ({condition.Condition} -> {preProcessed})");
            }
        }

        [TestMethod()]
        public void DirectoryCopy()
        {
            var conditions = new List<ExpectedConditon>()
            {
                new ExpectedConditon()
                {
                    Condition = @"('$DirectoryCopy(C:\Temp\DT_TEST, C:\Temp\DT_TEST_COPY, true)$' == '1')",
                    ExpectedResult = true
                },

                new ExpectedConditon()
                {
                    Condition = @"('$DirectoryCopy(C:\Temp\DT_TEST)$' == '1')",
                    ExpectedResult = false
                },
                new ExpectedConditon()
                {
                    Condition = @"('$DirectoryCopy(C:\Temp\DT_TEST, )$' == '1')",
                    ExpectedResult = false
                },
                new ExpectedConditon()
                {
                    Condition = @"('$DirectoryCopy( , )$' == '1')",
                    ExpectedResult = false
                },
                new ExpectedConditon()
                {
                    Condition = @"('$DirectoryCopy()$' == '1')",
                    ExpectedResult = false
                },
            };

            foreach (var condition in conditions)
            {
                var preProcessed = PreProcessor.Process(condition.Condition);
                var evaluated = Evaluation.Evaluate(preProcessed);
                Assert.AreEqual(condition.ExpectedResult, evaluated, $"Expected '{condition.ExpectedResult}' but got '{evaluated}' ({condition.Condition} -> {preProcessed})");
            }
        }

        [TestMethod()]
        public void DirectoryMove()
        {
            var conditions = new List<ExpectedConditon>()
            {
                new ExpectedConditon()
                {
                    Condition = @"('$DirectoryMove(C:\Temp\DT_TEST_COPY, C:\Temp\DT_TEST_MOVE, true)$' == '1')",
                    ExpectedResult = true
                },

                new ExpectedConditon()
                {
                    Condition = @"('$DirectoryMove(C:\Temp\DT_TEST)$' == '1')",
                    ExpectedResult = false
                },
                new ExpectedConditon()
                {
                    Condition = @"('$DirectoryMove(C:\Temp\DT_TEST, )$' == '1')",
                    ExpectedResult = false
                },
                new ExpectedConditon()
                {
                    Condition = @"('$DirectoryMove( , )$' == '1')",
                    ExpectedResult = false
                },
                new ExpectedConditon()
                {
                    Condition = @"('$DirectoryMove()$' == '1')",
                    ExpectedResult = false
                },
            };

            foreach (var condition in conditions)
            {
                var preProcessed = PreProcessor.Process(condition.Condition);
                var evaluated = Evaluation.Evaluate(preProcessed);
                Assert.AreEqual(condition.ExpectedResult, evaluated, $"Expected '{condition.ExpectedResult}' but got '{evaluated}' ({condition.Condition} -> {preProcessed})");
            }
        }

        [TestMethod()]
        public void DirectoryDelete()
        {
            var conditions = new List<ExpectedConditon>()
            {
                new ExpectedConditon()
                {
                    Condition = @"('$DirectoryDelete(C:\Temp\DT_TEST, true)$' == '1')",
                    ExpectedResult = true
                },
                new ExpectedConditon()
                {
                    Condition = @"('$DirectoryDelete(C:\Temp\DT_TEST_MOVE, true)$' == '1')",
                    ExpectedResult = true
                },

                new ExpectedConditon()
                {
                    Condition = @"('$DirectoryDelete()$' == '1')",
                    ExpectedResult = false
                }
            };

            foreach (var condition in conditions)
            {
                var preProcessed = PreProcessor.Process(condition.Condition);
                var evaluated = Evaluation.Evaluate(preProcessed);
                Assert.AreEqual(condition.ExpectedResult, evaluated, $"Expected '{condition.ExpectedResult}' but got '{evaluated}' ({condition.Condition} -> {preProcessed})");
            }
        }
    }
}
