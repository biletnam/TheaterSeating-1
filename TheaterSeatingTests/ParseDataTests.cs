using NUnit.Framework;

namespace TheaterSeating.Tests
{
    [TestFixture]
    public class ParseDataTests
    {
        IParseData _parseData;

        [SetUp]
        public void Setup()
        {
            _parseData = new ParseData();
        }

        [Test]
        public void ReturnsCorrectParsedLayout()
        {
            var input = @"6 6
                        3 5 5 3

                        Smith 2
                        Miller 12";

            var actual = _parseData.ParseOrder(input);

            Assert.AreEqual(2, actual.Layout.Rows.Count);
            Assert.AreEqual(5, actual.Layout.Rows[1][1]);
            Assert.AreEqual(6, actual.Layout.Rows[0][1]);
        }

        [Test]
        public void ReturnsCorrectParsedReservation()
        {
            var input = @"6 6
                        3 5 5 3

                        Smith 2
                        Miller 12";

            var actual = _parseData.ParseOrder(input);

            Assert.AreEqual("Miller", actual.Requests[1].CustomerName);
            Assert.AreEqual(12, actual.Requests[1].RequestCount);
        }

        [Test]
        public void ReturnsNullWhenInputIsInvalid()
        {
            var input = @"A 6 6
                        3 5 A 3
                        Smith 2
                        Miller 12";

            var actual = _parseData.ParseOrder(input);

            Assert.IsNull(actual);
        }
    }
}