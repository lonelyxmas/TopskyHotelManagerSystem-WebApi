namespace EOM.TSHotelManagement.Common.Contract
{
    public class ReadApplicationVersionOutputDto
    {
        public int Id { get; set; }
        public int ApplicationVersionId { get; set; }
        public string VersionNumber { get; set; }
        public string VersionDescription { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
}



