namespace EOM.TSHotelManagement.Common.Contract
{
    public class ReadReserInputDto : ListInputDto
    {
        public string ReservationId { get; set; }
        public string CustomerName { get; set; }
        public string ReservationPhoneNumber { get; set; }
        public string ReservationRoomNumber { get; set; }
        public string ReservationChannel { get; set; }
        public DateTime ReservationStartDate { get; set; }
        public DateTime ReservationEndDate { get; set; }
    }
}

