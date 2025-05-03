using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EOM.TSHotelManagement.Common.Contract
{
    public class DeleteAppointmentNoticeTypeInputDto:BaseInputDto
    {
        /// <summary>
        /// 公告类型编号 (AppointmentNotice Type Number)
        /// </summary>
        public string NoticeTypeNumber { get; set; }
    }
}
