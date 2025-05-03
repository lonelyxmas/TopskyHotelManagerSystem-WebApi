namespace EOM.TSHotelManagement.Common.Contract
{
    public class UpdatePositionInputDto : BaseInputDto
    {
        public int PositionId { get; set; }
        public string PositionNumber { get; set; }
        public string PositionName { get; set; }
        public string PositionDescription { get; set; }
    }
}



