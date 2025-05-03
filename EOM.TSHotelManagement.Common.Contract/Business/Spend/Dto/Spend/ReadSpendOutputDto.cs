using EOM.TSHotelManagement.Common.Util;

namespace EOM.TSHotelManagement.Common.Contract
{
    public class ReadSpendOutputDto
    {
        public int Id { get; set; }
        [UIDisplay("消费编号", false, false)]
        public string SpendNumber { get; set; }
        [UIDisplay("房间号")]
        public string RoomNumber { get; set; }
        [UIDisplay("客户编号")]
        public string CustomerNumber { get; set; }
        [UIDisplay("商品编号", false, false)]
        public string ProductNumber { get; set; }
        [UIDisplay("商品名称")]
        public string ProductName { get; set; }
        [UIDisplay("消费数量", true)]
        public int ConsumptionQuantity { get; set; }
        [UIDisplay("商品单价")]
        public decimal ProductPrice { get; set; }
        public string ProductPriceFormatted { get; set; }
        [UIDisplay("消费金额")]
        public decimal ConsumptionAmount { get; set; }
        public string ConsumptionAmountFormatted { get; set; }
        [UIDisplay("消费时间")]
        public DateTime ConsumptionTime { get; set; }
        public string SettlementStatus { get; set; }
        [UIDisplay("结算状态")]
        public string SettlementStatusDescription { get; set; }
    }
}

