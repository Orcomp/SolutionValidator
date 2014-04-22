// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IntegrationTest.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace SolutionValidator.Core.Tests.Validator.ProjectFile
{
    using System;
    using System.IO;
    using System.Linq;
    using NUnit.Framework;
    using SolutionValidator.Tests;
    using SolutionValidator.Validator;
    using SolutionValidator.Validator.Common;
    using SolutionValidator.Validator.ProjectFile;
    using SolutionValidator.Validator.ProjectFile.Rules;

    [TestFixture]
    public class IntegrationTest
    {
        #region Setup/Teardown

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
        }


        [SetUp]
        public void SetUp()
        {
            helper = new ProjectFileHelper();
        }

        [TearDown]
        public void TearDown()
        {
            try
            {
                if (tempRepoRoot == null)
                {
                    return;
                }
                if (Directory.Exists(tempRepoRoot))
                {
                    Directory.Delete(tempRepoRoot, true);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        #endregion

        private const string TestDataPath = "TestData";
        private ProjectFileHelper helper;
        private ProjectFileRule rule;
        private string tempRepoRoot;

        [Test]
        [TestCase("output", "p2.csproj", @"level1\level2", null, null, 0, null)]
        [TestCase("notoutput", "p2.csproj", @"level1\level2", null, null, 2, "notoutput;release;debug")]
        [TestCase("output", "p2.csproj", @"level1", @"..\..", @"..", 0, null)]
        [TestCase("output", "p2.csproj", @"level1\level2\level3", @"..\..", @"..\..\..", 0, null)]
        [TestCase("output", "p2.csproj", @"level1\level2", @"..\..", @"c:\\", 2, "must be a relative path")]
        [TestCase("output", "p2.csproj", @"level1\level2", @"..\..\output\Debug", @"..\..\output\xxx", 1, "debug;xxx")]
        [TestCase(@"output\custom", "p2.csproj", @"level1\level2", @"..\..\output", @"..\..\output\custom", 0, null)]
        public void TestAndCountOutPutPath(string outputPath, string projectNames, string subFolder, string patchFrom, string patchTo, int expectedCount, string expectedContains)
        {
            // Arrange:
            tempRepoRoot = PrepareSolution(projectNames, subFolder, patchFrom, patchTo);
            rule = new OutPutPathProjectFileRule(outputPath, helper);


            // Act:
            var result = rule.Validate(new RepositoryInfo(tempRepoRoot));

            // Assert:
            Assert.AreEqual(expectedCount, result.ErrorCount, result.RuleDescription);
            Assert.AreEqual(expectedCount == 0, result.IsValid);

            if (!string.IsNullOrEmpty(expectedContains))
            {
                foreach (var expected in expectedContains.Split(';'))
                {
                    var errorMessages = string.Concat(result.Messages.Where(m => m.ResultLevel == ResultLevel.Error).Select(m => m.Message)).Contains(expected);
                    Assert.IsTrue(errorMessages, string.Format("Expected contains: {0}, but was: {1}", expected, result.RuleDescription));
                }
            }
        }


        [Test]
        [TestCase("Debug", "p2.csproj", @"level1\level2", null, null, 0, "contains;Debug;p2")]
        [TestCase("Release", "p2.csproj", @"level1\level2", null, null, 0, "contains;Release;p2")]
        [TestCase("NotExisting", "p2.csproj", @"level1\level2", null, null, 1, "does not contain;NotExisting;p2")]
        public void TestAndCountConfigurationExists(string configNameToCheck, string projectNames, string subFolder, string patchFrom, string patchTo, int expectedCount, string expectedContains)
        {
            // Arrange:
            tempRepoRoot = PrepareSolution(projectNames, subFolder, patchFrom, patchTo);
            rule = new ConfigurationExistsProjectFileRule(configNameToCheck, helper);


            // Act:
            var result = rule.Validate(new RepositoryInfo(tempRepoRoot));

            // Assert:
            Assert.AreEqual(expectedCount, result.ErrorCount, result.RuleDescription);
            Assert.AreEqual(expectedCount == 0, result.IsValid);
            Assert.AreEqual(1, result.CheckCount);

            if (!string.IsNullOrEmpty(expectedContains))
            {
                if (expectedCount == 0)
                {
                    foreach (var expected in expectedContains.Split(';'))
                    {
                        var passedMessages = string.Concat(result.Messages.Where(m => m.ResultLevel == ResultLevel.Passed).Select(m => m.Message)).Contains(expected);
                        Assert.IsTrue(passedMessages, string.Format("Expected contains: {0}, but was: {1}", expected, result.RuleDescription));
                    }
                }
                else
                {
                    foreach (var expected in expectedContains.Split(';'))
                    {
                        var errorMessages = string.Concat(result.Messages.Where(m => m.ResultLevel == ResultLevel.Error).Select(m => m.Message)).Contains(expected);
                        Assert.IsTrue(errorMessages, string.Format("Expected contains: {0}, but was: {1}", expected, result.RuleDescription));
                    }
                }
            }
        }

        [Test]
        [TestCase("AssemblyName", "AssemblyName", "p2.csproj", @"level1\level2", null, null, 0, "are identical;AssemblyName")]
        [TestCase("AssemblyName", "AssemblyName", "p1.csproj", @"level1\level2", null, null, 0, "are identical;AssemblyName")]
        [TestCase("AssemblyName", "RootNamespace", "p1.csproj", @"level1\level2", null, null, 0, "are identical;AssemblyName;RootNamespace")]
        [TestCase("AssemblyName", "RootNamespace", "p2.csproj", @"level1\level2", null, null, 0, "are identical;AssemblyName;RootNamespace")]
        [TestCase("AssemblyName", "RootNamespace", "p1.csproj", @"level1\level2", "<AssemblyName>", "<AssemblyName>x", 1, "are not identical;AssemblyName;RootNamespace")]
        [TestCase("AssemblyNamex", "AssemblyName", "p1.csproj", @"level1\level2", null, null, 1, "are not identical;AssemblyNamex")]
        [TestCase("AssemblyNamex", "AssemblyNamex", "p1.csproj", @"level1\level2", null, null, 1, "are not identical;AssemblyNamex")]
        [TestCase("", "AssemblyName", "p1.csproj", @"level1\level2", null, null, 1, "are not identical;AssemblyName;<empty>")]
        [TestCase("AssemblyName", "", "p1.csproj", @"level1\level2", null, null, 1, "are not identical;AssemblyName;<empty>")]
        public void TestAndCountCheckIdentical(string propertyName, string otherPropertyName, string projectNames, string subFolder, string patchFrom, string patchTo, int expectedCount, string expectedContains)
        {
            // Arrange:
            tempRepoRoot = PrepareSolution(projectNames, subFolder, patchFrom, patchTo);
            rule = new CheckIdenticalProjectFileRule(propertyName, otherPropertyName, helper);

            // Act:
            var result = rule.Validate(new RepositoryInfo(tempRepoRoot));

            // Assert:
            Assert.AreEqual(expectedCount, result.ErrorCount, result.RuleDescription);
            Assert.AreEqual(expectedCount == 0, result.IsValid);
            Assert.AreEqual(1, result.CheckCount);

            if (!string.IsNullOrEmpty(expectedContains))
            {
                if (expectedCount == 0)
                {
                    foreach (var expected in expectedContains.Split(';'))
                    {
                        var passedMessages = string.Concat(result.Messages.Where(m => m.ResultLevel == ResultLevel.Passed).Select(m => m.Message)).Contains(expected);
                        Assert.IsTrue(passedMessages, string.Format("Expected contains: {0}, but was: {1}", expected, result.RuleDescription));
                    }
                }
                else
                {
                    foreach (var expected in expectedContains.Split(';'))
                    {
                        var errorMessages = string.Concat(result.Messages.Where(m => m.ResultLevel == ResultLevel.Error).Select(m => m.Message)).Contains(expected);
                        Assert.IsTrue(errorMessages, string.Format("Expected contains: {0}, but was: {1}", expected, result.RuleDescription));
                    }
                }
            }
        }


        [Test]
        [TestCase("Platform", "AnyCPU", "p1.csproj", @"level1\level2", null, null, 0, "has the expected;Platform;AnyCPU")]
        [TestCase("Platform", "BadValue", "p1.csproj", @"level1\level2", null, null, 1, "has unexpected value;Platform;AnyCPU;BadValue")]
        [TestCase("BadProperty", "BadValue", "p1.csproj", @"level1\level2", null, null, 1, "has unexpected value;BadProperty;BadValue")]
        [TestCase("", "BadValue", "p1.csproj", @"level1\level2", null, null, 1, "has unexpected value;<empty>;BadValue")]
        [TestCase("", "", "p1.csproj", @"level1\level2", null, null, 1, "has unexpected value;<empty>")]
        public void TestAndCountCheckForValue(string propertyName, string value, string projectNames, string subFolder, string patchFrom, string patchTo, int expectedCount, string expectedContains)
        {
            // Arrange:
            tempRepoRoot = PrepareSolution(projectNames, subFolder, patchFrom, patchTo);
            rule = new CheckForValueProjectFileRule(propertyName, value, helper);

            // Act:
            var result = rule.Validate(new RepositoryInfo(tempRepoRoot));

            // Assert:
            Assert.AreEqual(expectedCount, result.ErrorCount, result.RuleDescription);
            Assert.AreEqual(expectedCount == 0, result.IsValid);
            Assert.AreEqual(1, result.CheckCount);

            if (!string.IsNullOrEmpty(expectedContains))
            {
                if (expectedCount == 0)
                {
                    foreach (var expected in expectedContains.Split(';'))
                    {
                        var passedMessages = string.Concat(result.Messages.Where(m => m.ResultLevel == ResultLevel.Passed).Select(m => m.Message)).Contains(expected);
                        Assert.IsTrue(passedMessages, string.Format("Expected contains: {0}, but was: {1}", expected, result.RuleDescription));
                    }
                }
                else
                {
                    foreach (var expected in expectedContains.Split(';'))
                    {
                        var errorMessages = string.Concat(result.Messages.Where(m => m.ResultLevel == ResultLevel.Error).Select(m => m.Message)).Contains(expected);
                        Assert.IsTrue(errorMessages, string.Format("Expected contains: {0}, but was: {1}", expected, result.RuleDescription));
                    }
                }
            }
        }

        private string PrepareSolution(string projectNames, string subFolders, string patchFrom, string patchTo)
        {
            var tempFolder = TestUtils.CreateTempRootFolder();
            foreach (var projectName in projectNames.Split(';'))
            {
                var fileName = Path.GetFileName(projectName);
                var targetFolder = Path.Combine(tempFolder, subFolders);
                Directory.CreateDirectory(targetFolder);

                var targetFullName = Path.Combine(targetFolder, fileName);
                var sourceFullName = Path.GetFullPath(Path.Combine(TestDataPath, projectName));
                File.Copy(sourceFullName, targetFullName);
                PatchFile(targetFullName, patchFrom, patchTo);
            }
            return tempFolder;
        }

        private void PatchFile(string fileName, string patchFrom, string patchTo)
        {
            if (patchFrom == null || patchTo == null)
            {
                return;
            }
            string text = File.ReadAllText(fileName);
            text = text.Replace(patchFrom, patchTo);
            File.WriteAllText(fileName, text);
        }

        private static bool isInitialized;

    }
}