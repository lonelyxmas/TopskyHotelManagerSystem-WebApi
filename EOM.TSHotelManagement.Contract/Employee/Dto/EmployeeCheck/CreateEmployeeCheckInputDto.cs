using System.ComponentModel.DataAnnotations;

namespace EOM.TSHotelManagement.Contract
{
    public class CreateEmployeeCheckInputDto : BaseInputDto
    {
        /// <summary>
        /// 打卡编号 (Check-in/Check-out Number)
        /// </summary>
        [Required(ErrorMessage = "打卡编号为必填字段")]
        [MaxLength(128, ErrorMessage = "打卡编号长度不超过128字符")]
        public string CheckNumber { get; set; }
        /// <summary>
        /// 员工工号 (Employee ID)
        /// </summary>
        [Required(ErrorMessage = "员工工号为必填字段")]
        [MaxLength(128, ErrorMessage = "员工工号长度不超过128字符")]
        public string EmployeeId { get; set; }

        /// <summary>
        /// 打卡时间 (Check-in/Check-out Time)
        /// </summary>
        [Required(ErrorMessage = "打卡时间为必填字段")]
        public DateTime CheckTime { get; set; }

        /// <summary>
        /// 打卡方式 (Check-in/Check-out Method)
        /// </summary>
        [Required(ErrorMessage = "打卡方式为必填字段")]
        public string CheckMethod { get; set; }

        /// <summary>
        /// 打卡状态 (Check-in/Check-out Status)
        /// </summary>
        [Required(ErrorMessage = "打卡状态为必填字段")]
        public int CheckStatus { get; set; }
    }
}


