namespace EOM.TSHotelManagement.Common.Contract
{
    public class CreateModulePermissionInputDto : BaseInputDto
    {
        public int ModuleId { get; set; }
        public string PermissionName { get; set; }
        public string PermissionDescription { get; set; }
    }
}



