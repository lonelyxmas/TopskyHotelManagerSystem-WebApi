using System.ComponentModel.DataAnnotations;

namespace EOM.TSHotelManagement.Contract
{
    public class CreateSpendInputDto : BaseInputDto
    {
        [Required(ErrorMessage = "商品编号为必填字段")]
        public string ProductNumber { get; set; }

        [Required(ErrorMessage = "消费编号为必填字段")]
        public string SpendNumber { get; set; }

        public string RoomNumber { get; set; }

        public string CustomerNumber { get; set; }

        public string ProductName { get; set; }

        [Required(ErrorMessage = "消费数量为必填字段")]
        public int ConsumptionQuantity { get; set; }

        [Required(ErrorMessage = "商品单价为必填字段")]
        public decimal ProductPrice { get; set; }

        [Required(ErrorMessage = "消费金额为必填字段")]
        public decimal ConsumptionAmount { get; set; }

        [Required(ErrorMessage = "消费时间为必填字段")]
        public DateTime ConsumptionTime { get; set; }

        public string SettlementStatus { get; set; }

        public string ConsumptionType { get; set; }
    }
}

