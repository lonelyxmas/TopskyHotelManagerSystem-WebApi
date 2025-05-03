
using EOM.TSHotelManagement.Common.Util;

namespace EOM.TSHotelManagement.Common.Contract
{
    public class ReadCustomerOutputDto
    {
        public int Id { get; set; }
        [UIDisplay("客户编号")]
        public string CustomerNumber { get; set; }
        [UIDisplay("客户名称")]
        public string CustomerName { get; set; }
        [UIDisplay("性别", true, false)]
        public int? CustomerGender { get; set; }
        [UIDisplay("证件类型", true, false)]
        public int PassportId { get; set; }
        [UIDisplay("性别", false, true)]
        public string GenderName { get; set; }
        [UIDisplay("联系方式")]
        public string CustomerPhoneNumber { get; set; }
        [UIDisplay("出生日期")]
        public DateTime DateOfBirth { get; set; }
        [UIDisplay("客户类型", true, false)]
        public int CustomerType { get; set; }
        [UIDisplay("客户类型", false, true)]
        public string CustomerTypeName { get; set; }
        [UIDisplay("证件类型", false, true)]
        public string PassportName { get; set; }
        [UIDisplay("证件号码")]
        public string IdCardNumber { get; set; }
        [UIDisplay("客户地址")]
        public string CustomerAddress { get; set; }
    }
}

