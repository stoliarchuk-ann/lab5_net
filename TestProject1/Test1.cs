using lab5;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject1
{
    [TestClass]
    public class ExhibitTests
    {
        [TestMethod]
        public void Constructor_ValidData_SetsPropertiesCorrectly()
        {
            string name = "Ваза";
            string author = "Невідомий";
            int count_request = 15;

            Exhibit exhibit = new Exhibit(name, author, count_request);
            Assert.AreEqual(name, exhibit.Name);
            Assert.AreEqual(author, exhibit.Author);
            Assert.AreEqual(count_request, exhibit.Count);
        }

        [TestMethod]
        public void Count_SetNegativeValue_ThrowsArgumentException()
        {
            string name = "Ваза";
            string author = "Невідомий";
            int count_request = -5;

            Assert.ThrowsException<ArgumentException>(() => new Exhibit(name, author, count_request));
        }

        [TestMethod]
        public void Name_SetEmptyValue_ThrowsArgumentException()
        {
            string name = "";
            string author = "Невідомий";
            int count_request = 15;

            Assert.ThrowsException<ArgumentException>(() => new Exhibit(name, author, count_request));
        }

        [TestMethod]
        public void CompareTo_ComparesByName_ReturnsCorrectOrder()
        {
            string name1 = "А-експонат";
            string author1 = "Невідомий";
            int count_request1 = 15;
            string name2 = "Б-експонат";
            string author2 = "Невідомий";
            int count_request2 = 15;

            Exhibit exhibit1 = new Exhibit(name1, author1, count_request1);
            Exhibit exhibit2 = new Exhibit(name2, author2, count_request2);

            int result = exhibit1.CompareTo(exhibit2);
            Assert.IsTrue(result < 0);
        }

        [TestMethod]
        public void Count_SetZeroValue_SetsPropertlyCorrectly()
        {
            string name = "Ваза";
            string author = "Невідомий";
            int count_request = 0;

            Exhibit exhibit = new Exhibit(name, author, count_request);
            Assert.AreEqual(0, exhibit.Count);
        }

        [TestMethod]
        public void Author_SetEmptyValue_ThrowsArgumentException()
        {
            string name = "Ваза";
            string author = "   ";
            int count_request = 15;

            Assert.ThrowsException<ArgumentException>(() => new Exhibit(name, author, count_request));
        }

        [TestMethod]
        public void ToString_ReturnsFormattedString()
        {
            string name = "Ваза";
            string author = "Невідомий";
            int count_request = 15;

            Exhibit exhibit = new Exhibit(name, author, count_request);
            Assert.AreEqual("Назва: Ваза, автор: Невідомий, кількість заявок: 15.", exhibit.ToString());
        }

        [TestMethod]
        public void CompareTo_NullObject_ReturnsOne()
        {
            string name = "Ваза";
            string author = "Невідомий";
            int count_request = 15;

            Exhibit exhibit = new Exhibit(name, author, count_request);
            int result = exhibit.CompareTo(null);
            Assert.AreEqual(1, result);
        }
    }

    [TestClass]
    public class ExhibitCatalogTests
    {
        [TestMethod]
        public void AddExhibit_AddsItemToList()
        {
            ExhibitCatalog catalog = new ExhibitCatalog(new JsonFileWriter(), new JsonFileReader(), "test.json");

            string name = "Ваза";
            string author = "Невідомий";
            int count_request = 15;

            Exhibit exhibit = new Exhibit(name, author, count_request);

            catalog.AddExhibit(exhibit);
            Assert.AreEqual(1, catalog.Exhibits.Count);
        }

        [TestMethod]
        public void RemoveExhibit_RemovesItemFromList()
        {
            ExhibitCatalog catalog = new ExhibitCatalog(new JsonFileWriter(), new JsonFileReader(), "test.json");

            string name = "Ваза";
            string author = "Невідомий";
            int count_request = 15;

            Exhibit exhibit = new Exhibit(name, author, count_request);

            catalog.AddExhibit(exhibit);
            catalog.RemoveExhibit(exhibit);

            Assert.AreEqual(0, catalog.Exhibits.Count);
        }

        [TestMethod]
        public void FilterPopularExhibits_FiltersCorrectly()
        {
            ExhibitCatalog catalog = new ExhibitCatalog(new JsonFileWriter(), new JsonFileReader(), "test.json");

            Exhibit exhibit1 = new Exhibit("Ваза", "Невідомий", 5);
            Exhibit exhibit2 = new Exhibit("Картина", "Невідомий", 5);
            Exhibit exhibit3 = new Exhibit("Скульптура", "Невідомий", 2);

            catalog.AddExhibit(exhibit1);
            catalog.AddExhibit(exhibit2);
            catalog.AddExhibit(exhibit3);

            catalog.FilterPopularExhibits();

            Assert.AreEqual(2, catalog.Exhibits.Count);
        }

        [TestMethod]
        public void RemoveExhibit_ItemNotInList_DoesNothing()
        {
            ExhibitCatalog catalog = new ExhibitCatalog(new JsonFileWriter(), new JsonFileReader(), "test.json");

            string name = "Ваза";
            string author = "Невідомий";
            int count_request = 15;

            Exhibit exhibit = new Exhibit(name, author, count_request);
            Exhibit exhibit1 = new Exhibit(name, author, count_request);

            catalog.AddExhibit(exhibit);
            catalog.RemoveExhibit(exhibit1);

            Assert.AreEqual(1, catalog.Exhibits.Count);
        }

        [TestMethod]
        public void FilterPopularExhibits_EmptyCatalog_LeavesListEmpty()
        {
            ExhibitCatalog catalog = new ExhibitCatalog(new JsonFileWriter(), new JsonFileReader(), "test.json");

            catalog.FilterPopularExhibits();

            Assert.AreEqual(0, catalog.Exhibits.Count);
        }
    }

    [TestClass]
    public class ExhibitCountComparerTests
    {
        [TestMethod]
        public void Compare_FirstCountGreater_ReturnsPositive()
        {
            ExhibitCountComparer comparer = new ExhibitCountComparer();

            Exhibit exhibit1 = new Exhibit("Ваза", "Невідомий", 10);
            Exhibit exhibit2 = new Exhibit("Картина", "Невідомий", 5);

            int result = comparer.Compare(exhibit1, exhibit2);
            Assert.IsTrue(result > 0);
        }

        [TestMethod]
        public void Compare_FirstCountLess_ReturnsNegative()
        {
            ExhibitCountComparer comparer = new ExhibitCountComparer();

            Exhibit exhibit1 = new Exhibit("Ваза", "Невідомий", 5);
            Exhibit exhibit2 = new Exhibit("Картина", "Невідомий", 10);

            int result = comparer.Compare(exhibit1, exhibit2);
            Assert.IsTrue(result < 0);
        }

        [TestMethod]
        public void Compare_CountsEqual_ReturnsZero()
        {
            ExhibitCountComparer comparer = new ExhibitCountComparer();

            Exhibit exhibit1 = new Exhibit("Ваза", "Невідомий", 5);
            Exhibit exhibit2 = new Exhibit("Ваза", "Невідомий", 5);

            int result = comparer.Compare(exhibit1, exhibit2);
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void Compare_FirstObjectNull_ReturnsZero()
        {
            ExhibitCountComparer comparer = new ExhibitCountComparer();
            Exhibit exhibit = new Exhibit("Ваза", "Невідомий", 15);

            int result = comparer.Compare(null, exhibit);
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void Compare_SecondObjectNull_ReturnsZero()
        {
            ExhibitCountComparer comparer = new ExhibitCountComparer();
            Exhibit exhibit = new Exhibit("Ваза", "Невідомий", 15);

            int result = comparer.Compare(exhibit, null);
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void Compare_BothNull_ReturnsZero()
        {
            ExhibitCountComparer comparer = new ExhibitCountComparer();
            int result = comparer.Compare(null, null);
            Assert.AreEqual(0, result);
        }
    }

    [TestClass]
    public class FileHandlerTests
    {
        private string _testFilePath;

        [TestInitialize]
        public void TestInitialize()
        {
            _testFilePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + ".json");
        }

        [TestCleanup]
        public void TestCleanup()
        {
            if (File.Exists(_testFilePath))
            {
                File.Delete(_testFilePath);
            }
        }

        [TestMethod]
        public async Task SaveAsync_Should_WriteDataToFile()
        {
            List<Exhibit> list = new List<Exhibit>()
            {
                new Exhibit("Ваза", "Невідомий", 15)
            };

            var saver = new JsonFileWriter();
            await saver.SaveAsync(_testFilePath, list);

            Assert.IsTrue(File.Exists(_testFilePath));

            string fileContent = await File.ReadAllTextAsync(_testFilePath);
            Assert.IsFalse(string.IsNullOrWhiteSpace(fileContent));

            var reader = new JsonFileReader();
            var loadedList = await reader.LoadAsync<Exhibit>(_testFilePath);

            Assert.IsNotNull(loadedList);
            Assert.AreEqual(1, loadedList.Count);
        }

        [TestMethod]
        public async Task LoadAsync_Should_ReadDataFromFile()
        {
            string jsonContent = "[{\"Name\":\"Ваза\",\"Author\":\"Невідомий\",\"Count\":15}]";
            await File.WriteAllTextAsync(_testFilePath, jsonContent);

            var reader = new JsonFileReader();
            var loadedList = await reader.LoadAsync<Exhibit>(_testFilePath);

            Assert.IsNotNull(loadedList);
            Assert.AreEqual(1, loadedList.Count);
            Assert.AreEqual("Ваза", loadedList[0].Name);
        }

        [TestMethod]
        public async Task LoadAsync_WhenFileDoesNotExist_ReturnsNull()
        {
            var reader = new JsonFileReader();
            var loadedList = await reader.LoadAsync<Exhibit>(_testFilePath);

            Assert.IsNull(loadedList);
        }
    }
}