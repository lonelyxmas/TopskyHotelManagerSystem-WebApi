namespace EOM.TSHotelManagement.Common.Contract
{
    public class CreateEmployeeHistoryInputDto : BaseInputDto
    {
        public string EmployeeId { get; set; }
        public DateTime ChangeDate { get; set; }
        public string ChangeType { get; set; }
        public string ChangeDescription { get; set; }
    }
}


