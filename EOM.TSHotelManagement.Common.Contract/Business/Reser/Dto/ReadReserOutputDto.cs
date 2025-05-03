using EOM.TSHotelManagement.Common.Util;

namespace EOM.TSHotelManagement.Common.Contract
{
    public class ReadReserOutputDto
    {
        public int Id { get; set; }
        [UIDisplay("预约编号")]
        public string ReservationId { get; set; }
        [UIDisplay("客户姓名")]
        public string CustomerName { get; set; }
        [UIDisplay("联系方式")]
        public string ReservationPhoneNumber { get; set; }
        [UIDisplay("房间号码")]
        public string ReservationRoomNumber { get; set; }
        [UIDisplay("预约渠道")]
        public string ReservationChannel { get; set; }
        [UIDisplay("预约起始日")]
        public DateTime ReservationStartDate { get; set; }
        [UIDisplay("预约截止日")]
        public DateTime ReservationEndDate { get; set; }
    }
}

