namespace EOM.TSHotelManagement.Common.Contract
{
    public class CreateCustoTypeInputDto : BaseInputDto
    {
        /// <summary>
        /// 客户类型 (Customer Type)
        /// </summary>
        public string CustomerType { get; set; }

        /// <summary>
        /// 客户类型名称 (Customer Type Name)
        /// </summary>
        public string CustomerTypeName { get; set; }
        public decimal Discount { get; set; }
    }
}

