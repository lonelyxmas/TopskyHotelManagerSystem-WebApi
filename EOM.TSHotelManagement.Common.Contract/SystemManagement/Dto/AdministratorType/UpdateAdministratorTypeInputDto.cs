namespace EOM.TSHotelManagement.Common.Contract
{
    public class UpdateAdministratorTypeInputDto : BaseInputDto
    {
        public int Id { get; set; }
        public string TypeId { get; set; }
        public string TypeName { get; set; }
    }
}

