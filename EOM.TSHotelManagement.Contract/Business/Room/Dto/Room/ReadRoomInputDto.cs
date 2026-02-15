namespace EOM.TSHotelManagement.Contract
{
    public class ReadRoomInputDto : ListInputDto
    {
        public string? RoomNumber { get; set; }
        public int? RoomTypeId { get; set; }
        public int? RoomStateId { get; set; }
        public string? RoomName { get; set; }
        public DateOnly? LastCheckInTime { get; set; }
        public string? CustomerNumber { get; set; }
    }
}
