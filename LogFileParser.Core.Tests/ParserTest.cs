using System;
using LogFileParser.Common.LogFormats;
using NUnit.Framework;

namespace LogFileParser.Core.Tests
{
    public class ParserTest
    {
        private Parser _sut;

        [SetUp]
        public void Setup()
        {
            _sut = new Parser();
        }

        [Test]
        public void Should_Return_Parsed_Logs_For_Valid_W3C_Fields()
        {
            //Arrange
            var logFields = MockW3CFields().Split();

            //Act
            var result = _sut.Parse<W3CLogFormat>(logFields);

            //Assert
            Assert.AreEqual("172.22.255.255", result.ClientIpAddress);
            Assert.AreEqual("172.30.255.255", result.ServerIpAddress);
            Assert.AreEqual(80, result.ServerPort);
            Assert.AreEqual("GET", result.Method);
        }

        [Test]
        public void Should_Return_Parsed_Logs_Handling_Invalid_W3C_Fields_Executing_TryParse()
        {
            //Arrange
            var logFields = MockInvalidW3CFields().Split();

            //Act
            var result = _sut.TryParse<W3CLogFormat>(logFields);

            //Assert
            Assert.AreEqual(default(DateTime), result.Date);
            Assert.AreEqual(default(TimeSpan), result.Time);
            Assert.AreEqual(default(ushort), result.ServerPort);
            Assert.AreEqual(default(ushort), result.StatusCode);
            Assert.AreEqual(default(string), result.UriQuery);
            Assert.AreEqual("172.22.255.255", result.ClientIpAddress);
            Assert.AreEqual("172.30.255.255", result.ServerIpAddress);
            Assert.AreEqual("GET", result.Method);
        }

        [Test]
        public void Should_Return_Parsed_Logs_Handling_Valid_W3C_Fields_Executing_TryParse()
        {
            //Arrange
            var logFields = MockW3CFields().Split();

            //Act
            var result = _sut.TryParse<W3CLogFormat>(logFields);

            //Assert
            Assert.AreEqual("172.22.255.255", result.ClientIpAddress);
            Assert.AreEqual("172.30.255.255", result.ServerIpAddress);
            Assert.AreEqual(80, result.ServerPort);
            Assert.AreEqual("GET", result.Method);
        }

        private static string MockW3CFields()
        {
            return @"2002-05-02 17:42:15 172.22.255.255 - 172.30.255.255 80 " +
                     "GET /images/picture.jpg - 200 Mozilla/4.0+(compatible;MSIE+5.5;+Windows+2000+Server)";
        }

        private static string MockInvalidW3CFields()
        {
            return @"InvalidDate InvalidTime 172.22.255.255 - 172.30.255.255 InvalidPort " +
                     "GET  - InvalidStatus Mozilla/4.0+(compatible;MSIE+5.5;+Windows+2000+Server)";
        }
    }
}