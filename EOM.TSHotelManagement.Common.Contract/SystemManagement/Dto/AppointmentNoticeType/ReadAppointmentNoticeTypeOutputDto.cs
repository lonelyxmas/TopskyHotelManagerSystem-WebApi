using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EOM.TSHotelManagement.Common.Contract
{
    public class ReadAppointmentNoticeTypeOutputDto
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 公告类型编号 (AppointmentNotice Type Number)
        /// </summary>
        public string NoticeTypeNumber { get; set; }

        /// <summary>
        /// 公告类型名称 (AppointmentNotice Type Name)
        /// </summary>
        public string NoticeTypeName { get; set; }
        /// <summary>
        /// 删除标识
        /// </summary>
        public int? IsDelete { get; set; } = 0;
        /// <summary>
        /// 资料创建人
        /// </summary>
        public string DataInsUsr { get; set; }
        /// <summary>
        /// 资料创建时间
        /// </summary>
        public DateTime? DataInsDate { get; set; }
        /// <summary>
        /// 资料更新人
        /// </summary>
        public string DataChgUsr { get; set; }
        /// <summary>
        /// 资料更新时间
        /// </summary>
        public DateTime? DataChgDate { get; set; }
    }
}
