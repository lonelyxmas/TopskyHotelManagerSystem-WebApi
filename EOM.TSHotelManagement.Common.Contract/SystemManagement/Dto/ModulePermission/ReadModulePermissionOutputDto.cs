namespace EOM.TSHotelManagement.Common.Contract
{
    public class ReadModulePermissionOutputDto
    {
        public int Id { get; set; }
        /// <summary>
        /// 模块ID (Module ID)
        /// </summary>
        public int ModuleId { get; set; }

        /// <summary>
        /// 管理员账号 (Administrator Account)
        /// </summary>
        public string AdministratorAccount { get; set; }

        /// <summary>
        /// 模块名称 (Module Name)
        /// </summary>
        public string ModuleName { get; set; }

        /// <summary>
        /// 是否开启 (Is Enabled)
        /// </summary>
        public int ModuleEnabled { get; set; }
    }
}



