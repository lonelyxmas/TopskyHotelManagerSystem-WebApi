namespace EOM.TSHotelManagement.Common.Contract
{
    public class UpdateEmployeeHistoryInputDto : BaseInputDto
    {
        public int HistoryId { get; set; }
        public string EmployeeId { get; set; }
        public DateTime ChangeDate { get; set; }
        public string ChangeType { get; set; }
        public string ChangeDescription { get; set; }
    }
}


