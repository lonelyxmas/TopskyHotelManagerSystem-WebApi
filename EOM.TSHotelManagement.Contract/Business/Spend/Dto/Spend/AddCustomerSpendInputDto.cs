using System.ComponentModel.DataAnnotations;

namespace EOM.TSHotelManagement.Contract
{
    public class AddCustomerSpendInputDto
    {
        [Required(ErrorMessage = "房间编号为必填字段")]
        public string RoomNumber { get; set; }

        [Required(ErrorMessage = "商品编号为必填字段")]
        public string ProductNumber { get; set; }

        [Required(ErrorMessage = "商品名称为必填字段")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "数量为必填字段")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "价格为必填字段")]
        public decimal Price { get; set; }

        public string WorkerNo { get; set; }

        public string SoftwareVersion { get; set; }
    }
}
