using System;
using System.Linq;
using System.Collections.Generic;

namespace TheaterSeating
{
    public class ParseData : IParseData
    {
        public Order ParseOrder(string input)
        {   
            try
            {
                var parsedInput = input.Split(new string[] { "\n\n", "\r\n\r\n" }, StringSplitOptions.None);

                return new Order()
                {
                    Layout = ParseLayout(parsedInput[0]),
                    Requests = ParseReservations(parsedInput[1])
                };
            }
            catch
            {
                return null;
            }            
        }

        private Layout ParseLayout(string parsedInput)
        {
            var layout = new Layout()
            {
                Rows = new List<List<int>>()
            };
            foreach (var layoutLine in parsedInput.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.None))
            {
                var sections = layoutLine.Trim().Split(' ').Select(a => Convert.ToInt32(a)).ToList();
                layout.Rows.Add(sections);
            }
            
            return layout;
        }

        private List<Request> ParseReservations(string parsedInput)
        {
            var reservations = new List<Request>();
            foreach (var reservationLine in parsedInput.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.None))
            {
                reservations.Add(new Request()
                {
                    CustomerName = reservationLine.Trim().Split(' ')[0],
                    RequestCount = Convert.ToInt32(reservationLine.Trim().Split(' ')[1])
                });
            }
            return reservations;
        } 
    }
}
