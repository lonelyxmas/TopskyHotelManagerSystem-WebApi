namespace EOM.TSHotelManagement.Common.Contract
{
    public class CreatePassportTypeInputDto : BaseInputDto
    {
        public int PassportId { get; set; }
        public string PassportName { get; set; }
    }
}

