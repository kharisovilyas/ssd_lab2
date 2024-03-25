using Microsoft.VisualStudio.TestTools.UnitTesting;
using lab2.model;
using System.Collections.Generic;
using lab2.utils;
using Newtonsoft.Json;

namespace TestLab2
{
    [TestClass]
    public class MaxSumSequenceFinderTests
    {
        [TestMethod]
        public void FindMaxSumSequence_SingleElementList_ReturnsEmptySequence()
        {
            // Arrange
            List<double> numbers = new List<double> { 5 }; // Single element list

            // Act
            MaxSumSequenceFinder finder = new MaxSumSequenceFinder(numbers);
            finder.FindMaxSumSequence();

            // Assert
            Assert.IsTrue(finder.isResultEmpty);
        }

        [TestMethod]
        public void FindMaxSumSequence_AllNegativeNumbers_ReturnsEmptySequence()
        {
            // Arrange
            List<double> numbers = new List<double> { -3, -5, -10, -2 }; // All negative numbers

            // Act
            MaxSumSequenceFinder finder = new MaxSumSequenceFinder(numbers);
            finder.FindMaxSumSequence();

            // Assert
            Assert.IsTrue(finder.isResultEmpty);
        }

        [TestMethod]
        public void FindMaxSumSequence_PositiveAndNegativeNumbers_ReturnsNonEmptySequence()
        {
            // Arrange
            List<double> numbers = new List<double> { 1, -2, 3, 4, -1, 5, -6, 2 }; // Mix of positive and negative numbers

            // Act
            MaxSumSequenceFinder finder = new MaxSumSequenceFinder(numbers);
            finder.FindMaxSumSequence();

            // Assert
            Assert.IsFalse(finder.isResultEmpty);
            CollectionAssert.AreEqual(new List<double> { 3, 4, -1, 5 }, finder.MaxSumSequence);
            Assert.AreEqual(11, finder.MaxSum);
        }
    }
    [TestClass]
    public class SettingsManagerTests
    {
        private const string TestSettingsFileName = "test_startup_settings.txt";

        [TestInitialize]
        public void Initialize()
        {
            // Создаем тестовый файл настроек со значением true
            File.WriteAllText(TestSettingsFileName, "true");
        }

        [TestCleanup]
        public void Cleanup()
        {
            // Удаляем тестовый файл настроек
            File.Delete(TestSettingsFileName);
        }

       

    }
    [TestClass]
    public class DataParserTests
    {
        private const string TestFilePath = "test_data.json";

        [TestInitialize]
        public void Initialize()
        {
            // Создаем тестовый JSON-файл с данными
            MyData testData = new MyData
            {
                Numbers = new List<double> { 1.0, 2.0, 3.0, 4.0, 5.0 }
            };
            string jsonData = JsonConvert.SerializeObject(testData);
            File.WriteAllText(TestFilePath, jsonData);
        }

        [TestCleanup]
        public void Cleanup()
        {
            // Удаляем тестовый JSON-файл после выполнения тестов
            File.Delete(TestFilePath);
        }

        [TestMethod]
        public void Parse_ValidFilePath_ReturnsDataObject()
        {
            // Arrange
            DataParser parser = new DataParser();

            // Act
            MyData parsedData = parser.Parse(TestFilePath);

            // Assert
            Assert.IsNotNull(parsedData);
            CollectionAssert.AreEqual(new List<double> { 1.0, 2.0, 3.0, 4.0, 5.0 }, parsedData.Numbers);
        }

        [TestMethod]
        public void Parse_InvalidFilePath_ReturnsNull()
        {
            // Arrange
            DataParser parser = new DataParser();
            string invalidFilePath = "invalid_path.json";

            // Act
            MyData parsedData = parser.Parse(invalidFilePath);

            // Assert
            Assert.IsNull(parsedData);
        }
    }
}
