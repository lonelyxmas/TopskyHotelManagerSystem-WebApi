using System.ComponentModel.DataAnnotations;

namespace EOM.TSHotelManagement.Contract
{
    public class CreateReserInputDto : BaseInputDto
    {
        [Required(ErrorMessage = "预约编号为必填字段"), MaxLength(128, ErrorMessage = "预约编号长度不超过128字符")]
        public string ReservationId { get; set; }

        [Required(ErrorMessage = "客户名称为必填字段"), MaxLength(200, ErrorMessage = "客户名称长度不超过200字符")]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "预约电话为必填字段"), MaxLength(256, ErrorMessage = "预约电话长度不超过256字符")]
        public string ReservationPhoneNumber { get; set; }

        [Required(ErrorMessage = "预约房号为必填字段"), MaxLength(128, ErrorMessage = "预约房号长度不超过128字符")]
        public string ReservationRoomNumber { get; set; }

        [Required(ErrorMessage = "预约渠道为必填字段"), MaxLength(50, ErrorMessage = "预约渠道长度不超过50字符")]
        public string ReservationChannel { get; set; }

        [Required(ErrorMessage = "预约起始日期为必填字段")]
        public DateTime ReservationStartDate { get; set; }

        [Required(ErrorMessage = "预约结束日期为必填字段")]
        public DateTime ReservationEndDate { get; set; }
    }
}

