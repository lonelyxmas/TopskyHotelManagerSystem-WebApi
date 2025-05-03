namespace EOM.TSHotelManagement.Common.Contract
{
    public class DeleteEmployeePhotoInputDto : ListInputDto
    {
        public int PhotoId { get; set; }
        public string EmployeeId { get; set; }
    }
}


