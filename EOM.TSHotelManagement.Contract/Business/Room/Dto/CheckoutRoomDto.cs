using System.ComponentModel.DataAnnotations;

namespace EOM.TSHotelManagement.Contract
{
    public class CheckoutRoomDto : BaseInputDto
    {
        [Required(ErrorMessage = "房间编号为必填字段"), MaxLength(128, ErrorMessage = "房间编号长度不超过128字符")]
        public string RoomNumber { get; set; }

        [Required(ErrorMessage = "客户编号为必填字段"), MaxLength(128, ErrorMessage = "客户编号长度不超过128字符")]
        public string CustomerNumber { get; set; }

        [Required(ErrorMessage = "水费为必填字段")]
        public decimal WaterUsage { get; set; }

        [Required(ErrorMessage = "电费为必填字段")]
        public decimal ElectricityUsage { get; set; }
    }
}
