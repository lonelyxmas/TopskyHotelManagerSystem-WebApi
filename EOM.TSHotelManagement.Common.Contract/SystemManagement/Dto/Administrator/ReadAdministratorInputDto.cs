namespace EOM.TSHotelManagement.Common.Contract
{
    public class ReadAdministratorInputDto : ListInputDto
    {
        public int Id { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public int IsSuperAdmin { get; set; }
    }
}


