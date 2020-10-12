using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogFileParser.Common.LogFormats;
using NUnit.Framework;

namespace LogFileParser.Core.Tests
{
    public class FileParserTestW3C
    {
        private FileParser<W3CLogFormat> _sut;

        [SetUp]
        public void Setup()
        {
            _sut = new FileParser<W3CLogFormat>();
        }

        [Test]
        public async Task Should_Read_And_Parse_W3C_Logs_Async()
        {
            //Arrange
            string path = (TestContext.CurrentContext.TestDirectory + "\\Sample-logs\\W3C.log");

            //Act
            var result = await _sut.GetAllLogsAsync(path);

            //Assert
            Assert.AreEqual(1, result.Count);
        }

        [Test]
        public void Should_Throw_Exception_When_Invalid_File_Path_Is_Passed()
        {
            //Arrange
            string noExisitingPath = TestContext.CurrentContext.TestDirectory + "\\Sample-logs\\W3C1.log";

            //Act
            AsyncTestDelegate testDelegate = () => _sut.GetAllLogsAsync(noExisitingPath);

            //Assert
            Assert.ThrowsAsync<FileNotFoundException>(testDelegate);
        }
    }
}