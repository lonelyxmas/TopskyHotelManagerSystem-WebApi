namespace EOM.TSHotelManagement.Common.Contract
{
    public class CreateApplicationVersionInputDto : BaseInputDto
    {
        public string VersionNumber { get; set; }
        public string VersionDescription { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
}



