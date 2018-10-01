using NUnit.Framework;
using System.Collections.Generic;

namespace TheaterSeating.Tests
{
    [TestFixture]
    public class ProcessOrderTests
    {
        IProcessOrder _processOrder;

        [SetUp]
        public void Setup()
        {
            _processOrder = new ProcessOrder();
        }
        
        [Test]
        public void ReturnsCorrectTicketWhenPartyIsSmallEnough()
        {
            var order = new Order()
            {
                Layout = new Layout()
                {
                    Rows = new List<List<int>>()
                    {
                        new List<int>() { 3, 5, 5, 3 }
                    }
                },
                Requests = new List<Request>()
                {
                    new Request()
                    {
                        CustomerName = "Smith",
                        RequestCount = 2
                    }
                }
            };

            var actual = _processOrder.Process(order);

            Assert.AreEqual("Smith", actual[0].CustomerName);
            Assert.IsFalse(actual[0].IsPartyTooBig);
            Assert.IsFalse(actual[0].AskPartyToSplit);
            Assert.AreEqual(1, actual[0].AssignedSeat.RowNumber);
            Assert.AreEqual(2, actual[0].AssignedSeat.SectionNumber);

            // Ensure the assigned seat has correct remaining seats.
            Assert.AreEqual(1, order.Layout.Rows[0][0]);
        }

        [Test]
        public void ReturnsCorrectTicketWhenPartyIsSmallEnoughAndDoesntLeaveOneEmptySeatInASectionAndMovesToTheFrontWhenAvailable()
        {
            var order = new Order()
            {
                Layout = new Layout()
                {
                    Rows = new List<List<int>>()
                    {
                        new List<int>() { 3, 3, 3, 3 },
                        new List<int>() { 3, 5, 5, 3 }
                    }
                },
                Requests = new List<Request>()
                {
                    new Request()
                    {
                        CustomerName = "Johnson",
                        RequestCount = 2
                    }
                }
            };

            var actual = _processOrder.Process(order);

            Assert.AreEqual("Johnson", actual[0].CustomerName);
            Assert.IsFalse(actual[0].IsPartyTooBig);
            Assert.IsFalse(actual[0].AskPartyToSplit);
            Assert.AreEqual(1, actual[0].AssignedSeat.RowNumber);
            Assert.AreEqual(1, actual[0].AssignedSeat.SectionNumber);

            // Ensure the assigned seat has correct remaining seats.
            Assert.AreEqual(1, order.Layout.Rows[0][0]);

            // Ensure the original seat assigned is released.
            Assert.AreEqual(5, order.Layout.Rows[1][1]);            
        }

        [Test]
        public void ReturnsCorrectTicketWhenPartyIsSmallEnoughAndAssignsFullSectionSeatsFirstThenMovesToFrontWhenAvailable()
        {
            var order = new Order()
            {
                Layout = new Layout()
                {
                    Rows = new List<List<int>>()
                    {
                        new List<int>() { 3, 3, 5, 3, 3 },
                        new List<int>() { 3, 2, 2, 4, 2, 2, 3 }
                    }
                },
                Requests = new List<Request>()
                {
                    new Request()
                    {
                        CustomerName = "Wilson",
                        RequestCount = 4
                    }
                }
            };

            var actual = _processOrder.Process(order);

            Assert.AreEqual("Wilson", actual[0].CustomerName);
            Assert.IsFalse(actual[0].IsPartyTooBig);
            Assert.IsFalse(actual[0].AskPartyToSplit);
            Assert.AreEqual(1, actual[0].AssignedSeat.RowNumber);
            Assert.AreEqual(3, actual[0].AssignedSeat.SectionNumber);

            // Ensure the assigned seat has correct remaining seats.
            Assert.AreEqual(1, order.Layout.Rows[0][2]);

            // Ensure the original seat assigned is released.
            Assert.AreEqual(4, order.Layout.Rows[1][3]);            
        }

        [Test]
        public void ReturnsNoTicketWhenPartyCanBeSplit()
        {
            var order = new Order()
            {
                Layout = new Layout()
                {
                    Rows = new List<List<int>>()
                    {
                        new List<int>() { 3, 5, 5, 3 }
                    }
                },
                Requests = new List<Request>()
                {
                    new Request()
                    {
                        CustomerName = "Smith",
                        RequestCount = 6
                    }
                }
            };

            var actual = _processOrder.Process(order);

            Assert.AreEqual("Smith", actual[0].CustomerName);
            Assert.IsTrue(actual[0].AskPartyToSplit);
        }

        [Test]
        public void ReturnsNoTicketWhenPartyIsTooBig()
        {
            var order = new Order()
            {
                Layout = new Layout()
                {
                    Rows = new List<List<int>>()
                    {
                        new List<int>() { 3, 5, 5, 3 }
                    }
                },
                Requests = new List<Request>()
                {
                    new Request()
                    {
                        CustomerName = "Smith",
                        RequestCount = 17
                    }
                }
            };

            var actual = _processOrder.Process(order);

            Assert.AreEqual("Smith", actual[0].CustomerName);
            Assert.IsTrue(actual[0].IsPartyTooBig);
        }

        [Test]
        public void ReturnsNullWhenProcessingThrowsAnException()
        {
            var order = new Order();

            var actual = _processOrder.Process(order);

            Assert.IsNull(actual);            
        }
    }
}
