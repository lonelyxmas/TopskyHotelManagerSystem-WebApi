namespace EOM.TSHotelManagement.Common.Contract
{
    public class CreateRoomInputDto : BaseInputDto
    {
        public string RoomNumber { get; set; }
        public int RoomTypeId { get; set; }
        public string CustomerNumber { get; set; }
        public DateTime? LastCheckInTime { get; set; }
        public DateTime? LastCheckOutTime { get; set; }
        public int RoomStateId { get; set; }
        public decimal RoomRent { get; set; }
        public decimal RoomDeposit { get; set; }
        public string RoomLocation { get; set; }
    }
}

