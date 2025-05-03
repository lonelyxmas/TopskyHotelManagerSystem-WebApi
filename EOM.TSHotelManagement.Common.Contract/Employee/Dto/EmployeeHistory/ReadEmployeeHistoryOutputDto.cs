namespace EOM.TSHotelManagement.Common.Contract
{
    public class ReadEmployeeHistoryOutputDto
    {
        public int Id { get; set; }
        public string EmployeeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Position { get; set; }
        public string Company { get; set; }
    }
}


