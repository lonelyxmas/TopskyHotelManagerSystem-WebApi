using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EOM.TSHotelManagement.Common.Core
{
    [SugarTable("appointment_notice_type", "任命公告类型 (Appointment AppointmentNotice Type)")]
    public class AppointmentNoticeType : BaseEntity
    {
        /// <summary>
        /// 编号 (ID)
        /// </summary>
        [SugarColumn(ColumnName = "id", IsIdentity = true, IsPrimaryKey = true, IsNullable = false, ColumnDescription = "编号 (ID)")]
        public int Id { get; set; }

        /// <summary>
        /// 公告类型编号 (AppointmentNotice Type Number)
        /// </summary>
        [SugarColumn(ColumnName = "notice_type_number", IsPrimaryKey = true, IsNullable = false, Length = 128, ColumnDescription = "公告类型编号 (AppointmentNotice Type Number)")]
        public string NoticeTypeNumber { get; set; }

        /// <summary>
        /// 公告类型名称 (AppointmentNotice Type Name)
        /// </summary>
        [SugarColumn(ColumnName = "notice_type_name", IsNullable = false, Length = 200, ColumnDescription = "公告类型名称 (AppointmentNotice Type Name)")]
        public string NoticeTypeName { get; set; }
    }
}
