namespace EOM.TSHotelManagement.Common.Contract
{
    public class UpdateEmployeeRewardPunishmentInputDto : BaseInputDto
    {
        public int RewardPunishmentId { get; set; }
        public string EmployeeId { get; set; }
        public DateTime RewardPunishmentDate { get; set; }
        public string RewardPunishmentType { get; set; }
        public string RewardPunishmentDescription { get; set; }
    }
}


