namespace EOM.TSHotelManagement.Common.Contract
{
    public class ReadRoleOutputDto
    {
        public int Id { get; set; }
        /// <summary>
        /// 备  注:角色编码
        /// 默认值:
        ///</summary>
        public string RoleNumber { get; set; } = null!;

        /// <summary>
        /// 备  注:角色名字
        /// 默认值:
        ///</summary>
        public string RoleName { get; set; } = null!;

        /// <summary>
        /// 备  注:角色描述
        /// 默认值:
        ///</summary>
        public string? RoleDescription { get; set; }
    }
}


