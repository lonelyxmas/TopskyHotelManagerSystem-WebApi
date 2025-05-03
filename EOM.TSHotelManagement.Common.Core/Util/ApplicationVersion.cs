using SqlSugar;

namespace EOM.TSHotelManagement.Common.Core
{
    /// <summary>
    /// 应用版本 (Application Version)
    /// </summary>
    [SugarTable("app_version", "应用版本 (Application Version)")]
    public class ApplicationVersion
    {
        /// <summary>
        /// 流水号 (Sequence Number)
        /// </summary>
        [SugarColumn(ColumnName = "base_versionId", IsIdentity = true, IsPrimaryKey = true, ColumnDescription = "流水号 (Sequence Number)")]
        public int Id { get; set; }

        /// <summary>
        /// 版本号 (Version Number)
        /// </summary>
        [SugarColumn(ColumnName = "base_version", Length = 100, ColumnDescription = "版本号 (Version Number)")]
        public string VersionNumber { get; set; }
    }
}
