namespace EOM.TSHotelManagement.Common.Contract
{
    public class UpdateReserInputDto : BaseInputDto
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

