namespace EOM.TSHotelManagement.Common.Contract
{
    public class CreateSellThingInputDto : BaseInputDto
    {
        public string ProductNumber { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public string Specification { get; set; }
        public decimal Stock { get; set; }
    }
}


