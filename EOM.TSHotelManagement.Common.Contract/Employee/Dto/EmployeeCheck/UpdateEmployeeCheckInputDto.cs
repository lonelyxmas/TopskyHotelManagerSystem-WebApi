namespace EOM.TSHotelManagement.Common.Contract
{
    public class UpdateEmployeeCheckInputDto : BaseInputDto
    {
        public int CheckId { get; set; }
        public string EmployeeId { get; set; }
        public DateTime CheckDate { get; set; }
        public string CheckStatus { get; set; }
    }
}


