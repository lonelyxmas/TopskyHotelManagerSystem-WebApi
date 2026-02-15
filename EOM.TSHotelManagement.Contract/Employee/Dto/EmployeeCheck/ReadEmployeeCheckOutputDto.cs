namespace EOM.TSHotelManagement.Contract
{
    public class ReadEmployeeCheckOutputDto : BaseOutputDto
    {
        public string EmployeeId { get; set; }

        public DateTime CheckTime { get; set; }
        public string CheckStatus { get; set; }
        public string CheckMethod { get; set; }

        public bool MorningChecked { get; set; }

        public bool EveningChecked { get; set; }

        public int CheckDay { get; set; }
    }
}


