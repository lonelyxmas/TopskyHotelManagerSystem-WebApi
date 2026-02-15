using System;

namespace EOM.TSHotelManagement.Domain
{
    public class BaseEntity
    {
        /// <summary>
        /// 删除标识
        /// </summary>
        [SqlSugar.SugarColumn(ColumnName = "delete_mk", Length = 11, IsNullable = false, DefaultValue = "0")]
        public int? IsDelete { get; set; } = 0;
        /// <summary>
        /// 资料创建人
        /// </summary>
        [SqlSugar.SugarColumn(ColumnName = "datains_usr", Length = 128, IsOnlyIgnoreUpdate = true, IsNullable = true)]
        public string? DataInsUsr { get; set; }
        /// <summary>
        /// 资料创建时间
        /// </summary>
        [SqlSugar.SugarColumn(ColumnName = "datains_date", IsOnlyIgnoreUpdate = true, IsNullable = true)]
        public DateTime? DataInsDate { get; set; }
        /// <summary>
        /// 资料更新人
        /// </summary>
        [SqlSugar.SugarColumn(ColumnName = "datachg_usr", Length = 128, IsOnlyIgnoreInsert = true, IsNullable = true)]
        public string? DataChgUsr { get; set; }
        /// <summary>
        /// 资料更新时间
        /// </summary>
        [SqlSugar.SugarColumn(ColumnName = "datachg_date", IsOnlyIgnoreInsert = true, IsNullable = true)]
        public DateTime? DataChgDate { get; set; }
        /// <summary>
        /// Token
        /// </summary>
        [SqlSugar.SugarColumn(IsIgnore = true)]
        public string? UserToken { get; set; }
    }
}
