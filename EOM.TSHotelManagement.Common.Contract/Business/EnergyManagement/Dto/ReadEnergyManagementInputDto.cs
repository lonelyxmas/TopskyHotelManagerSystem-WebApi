namespace EOM.TSHotelManagement.Common.Contract
{
    public class ReadEnergyManagementInputDto : ListInputDto
    {
        public int Id { get; set; }
        public string InformationId { get; set; }
        public string RoomNo { get; set; }
        public DateTime? UseDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}

