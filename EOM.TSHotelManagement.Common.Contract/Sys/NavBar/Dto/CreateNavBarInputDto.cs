namespace EOM.TSHotelManagement.Common.Contract
{
    public class CreateNavBarInputDto : BaseInputDto
    {
        public string NavigationBarName { get; set; }
        public int NavigationBarOrder { get; set; }
        public string NavigationBarImage { get; set; }
        public string NavigationBarEvent { get; set; }
        public int MarginLeft { get; set; }
    }
}

