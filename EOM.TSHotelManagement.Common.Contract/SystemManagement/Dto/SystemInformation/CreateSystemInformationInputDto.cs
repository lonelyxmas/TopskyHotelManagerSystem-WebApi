namespace EOM.TSHotelManagement.Common.Contract
{
    public class CreateSystemInformationInputDto : BaseInputDto
    {
        public string InformationTitle { get; set; }
        public string InformationContent { get; set; }
        public DateTime InformationDate { get; set; }
    }
}



