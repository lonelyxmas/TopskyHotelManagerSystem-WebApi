using EOM.TSHotelManagement.Common.Util;

namespace EOM.TSHotelManagement.Common.Contract
{
    public class ReadEnergyManagementOutputDto
    {
        public int Id { get; set; }
        public string InformationId { get; set; }
        [UIDisplay("房间编号")]
        public string RoomNumber { get; set; }
        [UIDisplay("客户编号")]
        public string CustomerNumber { get; set; }
        [UIDisplay("开始日期")]
        public DateTime StartDate { get; set; }
        [UIDisplay("结束日期")]
        public DateTime EndDate { get; set; }
        [UIDisplay("电量")]
        public decimal PowerUsage { get; set; }
        [UIDisplay("水量")]
        public decimal WaterUsage { get; set; }
        [UIDisplay("记录员")]
        public string Recorder { get; set; }
    }
}

