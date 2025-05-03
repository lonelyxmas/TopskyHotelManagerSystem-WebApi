namespace EOM.TSHotelManagement.Common.Contract
{
    public class CreateVipLevelRuleInputDto : BaseInputDto
    {
        public int Id { get; set; }

        public string RuleSerialNumber { get; set; }

        public string RuleName { get; set; }

        public decimal RuleValue { get; set; }

        public int VipLevelId { get; set; }

        public string VipLevelName { get; set; }
    }
}



