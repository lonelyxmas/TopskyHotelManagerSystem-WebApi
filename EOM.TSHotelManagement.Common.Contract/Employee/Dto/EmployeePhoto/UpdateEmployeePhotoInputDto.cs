namespace EOM.TSHotelManagement.Common.Contract
{
    public class UpdateEmployeePhotoInputDto : BaseInputDto
    {
        public int PhotoId { get; set; }
        public string EmployeeId { get; set; }
        public string PhotoUrl { get; set; }
    }
}


