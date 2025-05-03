namespace EOM.TSHotelManagement.Common.Contract
{
    public class UpdateSpendInputDto : BaseInputDto
    {
        public string SpendNumber { get; set; }
        public string RoomNumber { get; set; }
        public string OriginalRoomNumber { get; set; }
        public string CustomerNumber { get; set; }
        public string ProductName { get; set; }
        public int ConsumptionQuantity { get; set; }
        public decimal ProductPrice { get; set; }
        public decimal ConsumptionAmount { get; set; }
        public DateTime ConsumptionTime { get; set; }
        public string SettlementStatus { get; set; }
    }
}

