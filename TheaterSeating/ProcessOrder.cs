using System.Collections.Generic;
using System.Linq;

namespace TheaterSeating
{
    public class ProcessOrder : IProcessOrder
    {
        public List<Ticket> Process(Order order)
        {
            try
            {
                var tickets = new List<Ticket>();

                // Assign Seats - Prioritize filling complete sections and avoid leaving one seat in a section
                AssignPrioritySeats(order, tickets);

                // Optimize Seat Assignments - Move assigned seats as close to screen as possible
                OptimizeSeatAssignments(order, tickets);                
                
                return tickets;
            }
            catch
            {
                return null;
            }            
        }

        private List<Ticket> AssignPrioritySeats(Order order, List<Ticket> tickets)
        {
            foreach (var request in order.Requests)
            {
                var ticket = new Ticket()
                {
                    CustomerName = request.CustomerName,
                    AskPartyToSplit = false,
                    IsPartyTooBig = false
                };
                var assignedSeat = ProcessSeatAssignment(order, request, out int availableSeats, false);
                assignedSeat.SeatCount = request.RequestCount;
                if (assignedSeat.RowNumber == 0 && assignedSeat.SectionNumber == 0)
                {

                    if (availableSeats < request.RequestCount)
                    {
                        ticket.IsPartyTooBig = true;
                    }
                    else
                    {
                        ticket.AskPartyToSplit = true;
                    }
                }
                else
                {
                    ticket.AssignedSeat = assignedSeat;
                }

                tickets.Add(ticket);
            }

            return tickets;
        }

        private List<Ticket> OptimizeSeatAssignments(Order order, List<Ticket> tickets)
        {
            // Process the tickets list sorted in descending order for better optimization            
            foreach (var ticket in tickets
                .OrderByDescending(a => a.AssignedSeat != null ? a.AssignedSeat.RowNumber : 0)
                .ThenByDescending(b => b.AssignedSeat != null ? b.AssignedSeat.SectionNumber : 0))
            {
                if (ticket.AssignedSeat != null)
                {
                    var request = new Request()
                    {
                        CustomerName = ticket.CustomerName,
                        RequestCount = ticket.AssignedSeat.SeatCount
                    };
                    var assignedSeat = ProcessSeatAssignment(order, request, out int availableSeats, true);
                    if (assignedSeat.RowNumber < ticket.AssignedSeat.RowNumber)
                    {
                        ticket.AssignedSeat = assignedSeat;

                        // Release the original seat assignment
                        order.Layout.Rows[ticket.AssignedSeat.RowNumber][ticket.AssignedSeat.SectionNumber] += request.RequestCount;
                    }
                }
            }

            return tickets;
        }

        private Seat ProcessSeatAssignment(Order order, Request request, out int availableSeats, bool optimize)
        {
            availableSeats = 0;
            var seat = new Seat
            {
                RowNumber = 0,
                SectionNumber = 0
            };
                        
            for (var row = 0; row < order.Layout.Rows.Count; row++)
            {
                for (var section = 0; section < order.Layout.Rows[row].Count; section++)
                {
                    if (order.Layout.Rows[row][section] >= request.RequestCount)
                    {
                        if (!optimize)
                        {
                            if (order.Layout.Rows[row][section] - request.RequestCount > 1 ||
                                order.Layout.Rows[row][section] - request.RequestCount == 0)
                            {
                                AssignSeat(order, request, row, section, seat);
                                break;
                            }
                        } else
                        {
                            AssignSeat(order, request, row, section, seat);
                            break;
                        }                        
                    }
                    availableSeats += order.Layout.Rows[row][section];
                }
                if (seat.RowNumber > 0) break;
            }            

            return seat;
        }

        private void AssignSeat(Order order, Request request, int row, int section, Seat seat)
        {
            order.Layout.Rows[row][section] -= request.RequestCount;
            seat.RowNumber = row + 1;
            seat.SectionNumber = section + 1;
        }
    }
}