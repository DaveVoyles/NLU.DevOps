﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace LanguageUnderstanding.ModelPerformance.Tests
{
    using System.Linq;
    using FluentAssertions;
    using Models;
    using NUnit.Framework;

    [TestFixture]
    internal static class ModelPerformanceTests
    {
        [Test]
        [Category("Intent")]
        [Category("Performance")]
        [TestCaseSource(typeof(ModelPerformanceTestCaseSource), "TestCases")]
        public static void CompareIntent(LabeledUtteranceTestCaseData testCaseData)
        {
            var actual = testCaseData.ActualUtterance;
            var expected = testCaseData.ExpectedUtterance;
            actual.Intent.Should().Be(expected.Intent);
        }

        [Test]
        [Category("Text")]
        [Category("Performance")]
        [TestCaseSource(typeof(ModelPerformanceTestCaseSource), "TestCases")]
        public static void CompareText(LabeledUtteranceTestCaseData testCaseData)
        {
            var actual = testCaseData.ActualUtterance;
            var expected = testCaseData.ExpectedUtterance;
            actual.Text.Should().Be(expected.Text);
        }

        [Test]
        [Category("Entities")]
        [Category("Performance")]
        [TestCaseSource(typeof(ModelPerformanceTestCaseSource), "ExpectedEntityTestCases")]
        public static void CompareExpectedEntity(EntityTestCaseData testCaseData)
        {
            var actual = testCaseData.ActualUtterance;
            var expectedEntity = testCaseData.ExpectedEntity;
            actual.Entities.Any(actualEntity => IsEntityMatch(expectedEntity, actualEntity)).Should().BeTrue();
        }

        [Test]
        [Category("Entities")]
        [Category("Performance")]
        [TestCaseSource(typeof(ModelPerformanceTestCaseSource), "ExpectedEntityValueTestCases")]
        public static void CompareExpectedEntityValue(EntityTestCaseData testCaseData)
        {
            var actual = testCaseData.ActualUtterance;
            var expectedEntity = testCaseData.ExpectedEntity;
            actual.Entities.Any(actualEntity => expectedEntity.EntityValue == actualEntity.EntityValue).Should().BeTrue();
        }

        [Test]
        [Category("Entities")]
        [Category("Performance")]
        [TestCaseSource(typeof(ModelPerformanceTestCaseSource), "ActualEntityTestCases")]
        public static void CompareExtractedEntity(EntityTestCaseData testCaseData)
        {
            var actual = testCaseData.ActualUtterance;
            var expectedEntity = testCaseData.ExpectedEntity;
            actual.Entities.Any(actualEntity => IsEntityMatch(expectedEntity, actualEntity)).Should().BeTrue();
        }

        private static bool IsEntityMatch(Entity expected, Entity actual)
        {
            return expected.MatchText == actual.MatchText
                || expected.MatchText == actual.EntityValue
                || expected.EntityValue == actual.EntityValue
                || expected.EntityValue == actual.MatchText;
        }
    }
}
