namespace EOM.TSHotelManagement.Common.Contract
{
    public class CreateEmployeeRewardPunishmentInputDto : BaseInputDto
    {
        public string EmployeeId { get; set; }
        public DateTime RewardPunishmentDate { get; set; }
        public string RewardPunishmentType { get; set; }
        public string RewardPunishmentDescription { get; set; }
    }
}


