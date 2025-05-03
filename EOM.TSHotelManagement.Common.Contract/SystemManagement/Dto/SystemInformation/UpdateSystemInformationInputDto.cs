namespace EOM.TSHotelManagement.Common.Contract
{
    public class UpdateSystemInformationInputDto : BaseInputDto
    {
        public int InformationId { get; set; }
        public string InformationTitle { get; set; }
        public string InformationContent { get; set; }
        public DateTime InformationDate { get; set; }
    }
}



