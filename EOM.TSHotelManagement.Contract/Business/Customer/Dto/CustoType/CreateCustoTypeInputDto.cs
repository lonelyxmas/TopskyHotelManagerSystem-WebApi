namespace EOM.TSHotelManagement.Contract
{
    public class CreateCustoTypeInputDto : BaseInputDto
    {
        /// <summary>
        /// 客户类型 (Customer Type)
        /// </summary>
        public int CustomerType { get; set; }

        /// <summary>
        /// 客户类型名称 (Customer Type Name)
        /// </summary>
        public string CustomerTypeName { get; set; }

        /// <summary>
        /// 优惠折扣
        /// </summary>
        public decimal Discount { get; set; }
    }
}

