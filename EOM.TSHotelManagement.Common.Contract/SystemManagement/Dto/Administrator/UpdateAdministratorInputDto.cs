namespace EOM.TSHotelManagement.Common.Contract
{
    public class UpdateAdministratorInputDto : BaseInputDto
    {
        public string Number { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public int IsSuperAdmin { get; set; }
    }
}


