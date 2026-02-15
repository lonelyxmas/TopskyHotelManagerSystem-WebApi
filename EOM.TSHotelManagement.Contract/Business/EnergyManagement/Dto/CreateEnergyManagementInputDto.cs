using System.ComponentModel.DataAnnotations;

namespace EOM.TSHotelManagement.Contract
{
    public class CreateEnergyManagementInputDto : BaseInputDto
    {
        [Required(ErrorMessage = "信息编号为必填字段"), MaxLength(128, ErrorMessage = "信息编号长度不超过128字符")]
        public string InformationNumber { get; set; }

        [Required(ErrorMessage = "房间编号为必填字段"), MaxLength(128, ErrorMessage = "房间编号长度不超过128字符")]
        public string RoomNumber { get; set; }

        [Required(ErrorMessage = "客户编号为必填字段"), MaxLength(128, ErrorMessage = "客户编号长度不超过128字符")]
        public string CustomerNumber { get; set; }

        [Required(ErrorMessage = "开始日期为必填字段")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "结束日期为必填字段")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "电费为必填字段")]
        public decimal PowerUsage { get; set; }

        [Required(ErrorMessage = "水费为必填字段")]
        public decimal WaterUsage { get; set; }

        [Required(ErrorMessage = "记录员为必填字段"), MaxLength(150, ErrorMessage = "记录员长度不超过150字符")]
        public string Recorder { get; set; }
    }
}

