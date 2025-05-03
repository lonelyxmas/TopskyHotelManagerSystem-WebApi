namespace EOM.TSHotelManagement.Common.Contract
{
    public class DeleteRoleInputDto : BaseInputDto
    {
        /// <summary>
        /// 备  注:角色编码
        /// 默认值:
        ///</summary>
        public string RoleNumber { get; set; } = null!;
    }
}


