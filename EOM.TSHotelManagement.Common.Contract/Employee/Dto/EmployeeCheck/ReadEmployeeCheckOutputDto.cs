namespace EOM.TSHotelManagement.Common.Contract
{
    public class ReadEmployeeCheckOutputDto
    {
        public int Id { get; set; }
        public string EmployeeId { get; set; }
        public DateTime CheckTime { get; set; }
        public string CheckStatus { get; set; }
        public string CheckMethod { get; set; }

        public bool IsChecked { get; set; }
        public int CheckDay { get; set; }
    }
}


