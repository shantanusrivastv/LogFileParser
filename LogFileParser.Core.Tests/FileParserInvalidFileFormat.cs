﻿using System;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace LogFileParser.Core.Tests
{
    public class FileParserInvalidFileFormat
    {
        private FileParser<object> _sut;

        [SetUp]
        public void Setup()
        {
            _sut = new FileParser<object>(new LogParser());
        }

        [Test]
        public void Should_Throw_Exception_When_Invalid_File_Format_Is_Passed()
        {
            //Arrange
            string path = TestContext.CurrentContext.TestDirectory + "\\Sample-logs\\W3C";

            //Act
            Task AsyncTestDelegate() => _sut.GetAllLogsAsync(path);

            //Assert
            var aggregateException = Assert.ThrowsAsync<AggregateException>(AsyncTestDelegate);
            var innerException = aggregateException.InnerExceptions
                                        .FirstOrDefault();

            Assert.That(innerException, Is.Not.Null);
            Assert.That(innerException.GetType(), Is.EqualTo(typeof(ArgumentException)));
            Assert.That(innerException.Message, Is.EqualTo("Unsupported Type"));
        }
    }
}