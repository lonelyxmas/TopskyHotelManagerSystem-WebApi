using System.ComponentModel.DataAnnotations;

namespace EOM.TSHotelManagement.Contract
{
    public class TransferRoomDto : BaseInputDto
    {
        [Required(ErrorMessage = "源房间编号为必填字段"), MaxLength(128, ErrorMessage = "源房间编号长度不超过128字符")]
        public string OriginalRoomNumber { get; set; }

        [Required(ErrorMessage = "目标房间编号为必填字段"), MaxLength(128, ErrorMessage = "目标房间编号长度不超过128字符")]
        public string TargetRoomNumber { get; set; }

        [Required(ErrorMessage = "客户编号为必填字段"), MaxLength(128, ErrorMessage = "客户编号长度不超过128字符")]
        public string CustomerNumber { get; set; }
    }
}
