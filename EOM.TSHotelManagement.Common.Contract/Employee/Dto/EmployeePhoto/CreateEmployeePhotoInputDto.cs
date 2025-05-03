namespace EOM.TSHotelManagement.Common.Contract
{
    public class CreateEmployeePhotoInputDto : BaseInputDto
    {
        public string EmployeeId { get; set; }
        public string PhotoUrl { get; set; }
    }
}


