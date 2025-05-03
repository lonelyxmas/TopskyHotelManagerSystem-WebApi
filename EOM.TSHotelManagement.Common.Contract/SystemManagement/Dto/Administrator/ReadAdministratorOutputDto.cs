namespace EOM.TSHotelManagement.Common.Contract
{
    public class ReadAdministratorOutputDto : BaseDto
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public int IsSuperAdmin { get; set; }

        /// <summary>
        /// 是否为超级管理员描述 (Is Super Administrator Description)
        /// </summary>
        public string IsSuperAdminDescription { get; set; }

        /// <summary>
        /// 管理员类型名称 (Administrator Type Name)
        /// </summary>
        public string TypeName { get; set; }
    }
}


