using EOM.TSHotelManagement.Common.Util;

namespace EOM.TSHotelManagement.Common.Contract
{
    public class ReadSellThingOutputDto
    {
        public int Id { get; set; }
        [UIDisplay("商品编号")]
        public string ProductNumber { get; set; }
        [UIDisplay("商品名称")]
        public string ProductName { get; set; }
        [UIDisplay("商品价格")]
        public decimal ProductPrice { get; set; }
        [UIDisplay("规格型号")]
        public string Specification { get; set; }
        [UIDisplay("库存")]
        public decimal Stock { get; set; }
    }
}


