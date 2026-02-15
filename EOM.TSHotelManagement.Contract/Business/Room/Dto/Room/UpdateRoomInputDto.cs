namespace EOM.TSHotelManagement.Contract
{
    public class UpdateRoomInputDto : BaseInputDto
    {
        public string RoomNumber { get; set; }
        public int RoomTypeId { get; set; }
        public string CustomerNumber { get; set; }
        public DateOnly? LastCheckInTime { get; set; }
        public DateOnly? LastCheckOutTime { get; set; }
        public int RoomStateId { get; set; }
        public decimal RoomRent { get; set; }
        public decimal RoomDeposit { get; set; }
        public string RoomLocation { get; set; }
    }
}


