namespace SystemProject_Hotel_Management_.Models
{
    public class ReservationConfirmationViewModel
    {
        public string ReservationNumber { get; set; }
        public string UserName { get; set; }
        public DateTime ReservationDate { get; set; }
        public string ReservationTime { get; set; }
        public int PartySize { get; set; }
        public int TableNumber { get; set; }
    }

}
