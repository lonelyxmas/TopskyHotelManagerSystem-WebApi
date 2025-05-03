namespace EOM.TSHotelManagement.Common.Contract
{
    public class ReadRoomInputDto : ListInputDto
    {
        public string RoomNumber { get; set; }
        public int RoomStateId { get; set; }
        public string RoomTypeName { get; set; }
        public DateTime LastCheckInTime { get; set; }
        public string CustomerNumber { get; set; }
    }
}

