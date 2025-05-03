namespace EOM.TSHotelManagement.Common.Contract
{
    public class CreateEnergyManagementInputDto : BaseInputDto
    {
        public string InformationNumber { get; set; }
        public string RoomNumber { get; set; }
        public string CustomerNumber { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal PowerUsage { get; set; }
        public decimal WaterUsage { get; set; }
        public string Recorder { get; set; }
    }
}

