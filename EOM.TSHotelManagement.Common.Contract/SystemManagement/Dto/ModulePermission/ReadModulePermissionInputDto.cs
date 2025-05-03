namespace EOM.TSHotelManagement.Common.Contract
{
    public class ReadModulePermissionInputDto : ListInputDto
    {
        public int PermissionId { get; set; }
        public string Account { get; set; }
    }
}



