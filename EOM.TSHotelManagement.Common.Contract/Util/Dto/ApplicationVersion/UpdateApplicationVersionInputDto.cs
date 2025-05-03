namespace EOM.TSHotelManagement.Common.Contract
{
    public class UpdateApplicationVersionInputDto : BaseInputDto
    {
        public int VersionId { get; set; }
        public string VersionNumber { get; set; }
        public string VersionDescription { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
}



