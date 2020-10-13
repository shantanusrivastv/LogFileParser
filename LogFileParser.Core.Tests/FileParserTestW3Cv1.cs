using System.IO;
using System.Threading.Tasks;
using LogFileParser.Common.LogFileFormats;
using NUnit.Framework;

namespace LogFileParser.Core.Tests
{
    public class FileParserTestW3Cv1
    {
        private FileParser<W3Cv1LogFormat> _sut;

        [SetUp]
        public void Setup()
        {
            _sut = new FileParser<W3Cv1LogFormat>(new LogParser());
        }

        [Test]
        public async Task Should_Read_And_Parse_W3C_Logs_Async()
        {
            //Arrange
            string path = TestContext.CurrentContext.TestDirectory + "\\Sample-logs\\W3Cv1";

            //Act
            var result = await _sut.GetAllLogsAsync(path);

            //Assert
            Assert.AreEqual(2, result.Count);
        }

        [Test]
        public void Should_Throw_Exception_When_Invalid_File_Path_Is_Passed()
        {
            //Arrange
            string nonExistingPath = TestContext.CurrentContext.TestDirectory + "\\Sample-logs2\\W3Cv1";

            //Act
            Task AsyncTestDelegate() => _sut.GetAllLogsAsync(nonExistingPath);

            //Assert
            Assert.ThrowsAsync<DirectoryNotFoundException>(AsyncTestDelegate);
        }
    }
}