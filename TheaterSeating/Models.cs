using System.Collections.Generic;

namespace TheaterSeating
{
    public class Order
    {
        public Layout Layout { get; set; }
        public List<Request> Requests { get; set; }
    }

    public class Layout
    {
        public List<List<int>> Rows { get; set; }
    }
    
    public class Ticket
    {
        public bool AskPartyToSplit { get; set; }
        public bool IsPartyTooBig { get; set; }
        public string CustomerName { get; set; }
        public Seat AssignedSeat { get; set; }

    }

    public class Seat
    {
        public int RowNumber { get; set; }
        public int SectionNumber { get; set; }
        public int SeatCount { get; set; }
    }   

    public class Request
    {
        public string CustomerName { get; set; }
        public int RequestCount { get; set; }
    }
}