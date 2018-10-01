using System;

namespace TheaterSeating
{
    public class Main
    {
        private readonly IParseData _parseData;
        private readonly IProcessOrder _processOrder;

        public Main()
        {
            _parseData = new ParseData();
            _processOrder = new ProcessOrder();
        }

        public void Start()
        {
            Console.WriteLine("Theater Seating Program");

            var input = System.IO.File.ReadAllText(@"Input.txt");

            Console.WriteLine("\nInput:\n\n{0}\n\nOutput:\n", input);

            var order = _parseData.ParseOrder(input);    
            
            if (order != null)
            {
                var tickets = _processOrder.Process(order);
                foreach(var ticket in tickets)
                {
                    if (ticket.AskPartyToSplit)
                    {
                        Console.WriteLine("{0} Call to split party.", ticket.CustomerName);
                    }
                    else if (ticket.IsPartyTooBig)
                    {
                        Console.WriteLine("{0} Sorry, we can't handle your party.", ticket.CustomerName);
                    }
                    else
                    {
                        Console.WriteLine("{0} Row {1} Section {2}", ticket.CustomerName, ticket.AssignedSeat.RowNumber, ticket.AssignedSeat.SectionNumber);
                    }
                }
            } else
            {
                Console.WriteLine("There was a problem parsing your order.\nPlease double check your input.");
            }

            Console.ReadKey();
        }
    }
}
