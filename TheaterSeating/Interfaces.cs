using System.Collections.Generic;

namespace TheaterSeating
{
    public interface IParseData
    {
        Order ParseOrder(string input);
    }

    public interface IProcessOrder
    {
        List<Ticket> Process(Order order);
    }
}