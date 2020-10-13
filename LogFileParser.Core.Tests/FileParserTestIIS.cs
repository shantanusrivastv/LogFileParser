using System.IO;
using System.Threading.Tasks;
using LogFileParser.Common.LogFileFormats;
using NUnit.Framework;

namespace LogFileParser.Core.Tests
{
    public class FileParserTestIIS
    {
        private FileParser<IISLogFormat> _sut;

        [SetUp]
        public void Setup()
        {
            _sut = new FileParser<IISLogFormat>(new LogParser());
        }

        [Test]
        public async Task Should_Read_And_Parse_IIS_Logs_Async()
        {
            //Arrange
            string path = TestContext.CurrentContext.TestDirectory + "\\Sample-logs\\IIS";

            //Act
            var result = await _sut.GetAllLogsAsync(path);

            //Assert
            Assert.AreEqual(1, result.Count);
        }

        [Test]
        public void Should_Throw_Exception_When_Invalid_File_Path_Is_Passed()
        {
            //Arrange
            string nonExistingPath = TestContext.CurrentContext.TestDirectory + "\\Sample-logs2\\IIS1.log";

            //Act
            Task AsyncTestDelegate() => _sut.GetAllLogsAsync(nonExistingPath);

            //Assert
            Assert.ThrowsAsync<DirectoryNotFoundException>(AsyncTestDelegate);
        }
    }
}