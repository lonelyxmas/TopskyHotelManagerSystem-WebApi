namespace EOM.TSHotelManagement.Common.Contract
{
    public class CreateEmployeeCheckInputDto : BaseInputDto
    {
        /// <summary>
        /// 打卡编号 (Check-in/Check-out Number)
        /// </summary>
        public string CheckNumber { get; set; }
        /// <summary>
        /// 员工工号 (Employee ID)
        /// </summary>
        public string EmployeeId { get; set; }

        /// <summary>
        /// 打卡时间 (Check-in/Check-out Time)
        /// </summary>
        public DateTime CheckTime { get; set; }

        /// <summary>
        /// 打卡方式 (Check-in/Check-out Method)
        /// </summary>
        public string CheckMethod { get; set; }

        /// <summary>
        /// 打卡状态 (Check-in/Check-out Status)
        /// </summary>
        public int CheckStatus { get; set; }
    }
}


