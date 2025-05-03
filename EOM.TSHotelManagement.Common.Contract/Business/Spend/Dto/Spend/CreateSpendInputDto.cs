namespace EOM.TSHotelManagement.Common.Contract
{
    public class CreateSpendInputDto : BaseInputDto
    {
        public string ProductNumber { get; set; }
        public string SpendNumber { get; set; }
        public string RoomNumber { get; set; }
        public string CustomerNumber { get; set; }
        public string ProductName { get; set; }
        public int ConsumptionQuantity { get; set; }
        public decimal ProductPrice { get; set; }
        public decimal ConsumptionAmount { get; set; }
        public DateTime ConsumptionTime { get; set; }
        public string SettlementStatus { get; set; }
    }
}

