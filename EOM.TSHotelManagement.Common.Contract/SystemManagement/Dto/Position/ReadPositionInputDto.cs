namespace EOM.TSHotelManagement.Common.Contract
{
    public class ReadPositionInputDto : ListInputDto
    {
        public int PositionId { get; set; }
        public string PositionName { get; set; }
        public string PositionDescription { get; set; }
    }
}



