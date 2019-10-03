namespace Ticketing.Models
{
    public class ShowTicketLogMessage
    {
        public string ShowName { get; }
        public string Date { get; }

        public string TicketId { get; }

        public ShowTicketLogMessage(string showName, string date, string ticketId)
        {
            ShowName = showName;
            Date = date;
            TicketId = ticketId;
        }
    }
}