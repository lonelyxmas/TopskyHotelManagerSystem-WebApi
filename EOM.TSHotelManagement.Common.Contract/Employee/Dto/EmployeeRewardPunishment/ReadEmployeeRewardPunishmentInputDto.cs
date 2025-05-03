namespace EOM.TSHotelManagement.Common.Contract
{
    public class ReadEmployeeRewardPunishmentInputDto : ListInputDto
    {
        public int RewardPunishmentId { get; set; }
        public string EmployeeId { get; set; }
    }
}


