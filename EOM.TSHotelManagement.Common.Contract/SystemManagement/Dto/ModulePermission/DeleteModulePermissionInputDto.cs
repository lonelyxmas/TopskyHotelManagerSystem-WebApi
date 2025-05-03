namespace EOM.TSHotelManagement.Common.Contract
{
    public class DeleteModulePermissionInputDto : BaseInputDto
    {
        public int PermissionId { get; set; }
        public string AdministratorAccount { get; set; }
    }
}



