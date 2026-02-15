using System.ComponentModel.DataAnnotations;

namespace EOM.TSHotelManagement.Contract
{
    public class UpdateEmployeeHistoryInputDto : BaseInputDto
    {
        [Required(ErrorMessage = "履历ID为必填字段")]
        public int HistoryId { get; set; }

        [Required(ErrorMessage = "员工工号为必填字段")]
        [MaxLength(128, ErrorMessage = "员工工号长度不超过128字符")]
        public string EmployeeId { get; set; }

        [Required(ErrorMessage = "变更日期为必填字段")]
        public DateTime ChangeDate { get; set; }

        [MaxLength(128, ErrorMessage = "变更类型长度不超过128字符")]
        public string ChangeType { get; set; }

        [MaxLength(500, ErrorMessage = "变更描述长度不超过500字符")]
        public string ChangeDescription { get; set; }
    }
}


