namespace EOM.TSHotelManagement.Common.Contract
{
    public class UpdateModulePermissionInputDto : BaseInputDto
    {
        public int PermissionId { get; set; }
        public int ModuleId { get; set; }
        public string PermissionName { get; set; }
        public string PermissionDescription { get; set; }
    }
}



