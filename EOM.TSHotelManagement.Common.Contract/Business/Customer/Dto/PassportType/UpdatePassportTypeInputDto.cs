namespace EOM.TSHotelManagement.Common.Contract
{
    public class UpdatePassportTypeInputDto : BaseInputDto
    {
        public int PassportId { get; set; }
        public string PassportName { get; set; }
    }
}

