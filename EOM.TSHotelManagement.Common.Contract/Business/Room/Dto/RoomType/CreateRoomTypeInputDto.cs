namespace EOM.TSHotelManagement.Common.Contract
{
    public class CreateRoomTypeInputDto : BaseInputDto
    {
        public int RoomTypeId { get; set; }
        public string RoomTypeName { get; set; }
        public decimal RoomRent { get; set; }
        public decimal RoomDeposit { get; set; }
    }
}


