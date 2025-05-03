namespace EOM.TSHotelManagement.Common.Contract
{
    public class CreatePositionInputDto : BaseInputDto
    {
        public string PositionNumber { get; set; }
        public string PositionName { get; set; }
        public string PositionDescription { get; set; }
    }
}



