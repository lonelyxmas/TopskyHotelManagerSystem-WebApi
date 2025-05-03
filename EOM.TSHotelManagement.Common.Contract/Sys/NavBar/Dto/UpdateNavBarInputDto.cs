namespace EOM.TSHotelManagement.Common.Contract
{
    public class UpdateNavBarInputDto : BaseInputDto
    {
        public int NavigationBarId { get; set; }
        public string NavigationBarName { get; set; }
        public int NavigationBarOrder { get; set; }
        public string NavigationBarImage { get; set; }
        public string NavigationBarEvent { get; set; }
        public int MarginLeft { get; set; }
    }
}

