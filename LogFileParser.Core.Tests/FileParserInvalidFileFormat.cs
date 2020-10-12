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
    public class FileParserInvalidFileFormat
    {
        private FileParser<object> _sut;

        [SetUp]
        public void Setup()
        {
            _sut = new FileParser<object>();
        }

        [Test]
        public void Should_Throw_Exception_When_Invalid_File_Format_Is_Passed()
        {
            //Arrange
            string path = TestContext.CurrentContext.TestDirectory + "\\Sample-logs\\W3C.log";

            //Act
            AsyncTestDelegate testDelegate = () => _sut.GetAllLogsAsync(path);

            //Assert
            var aggregateException = Assert.ThrowsAsync<AggregateException>(testDelegate);
            var innerException = aggregateException.InnerExceptions
                                        .FirstOrDefault();

            Assert.That(innerException, Is.Not.Null);
            Assert.That(innerException.GetType(), Is.EqualTo(typeof(ArgumentException)));
            Assert.That(innerException.Message, Is.EqualTo("Unsupported Type"));
        }
    }
}