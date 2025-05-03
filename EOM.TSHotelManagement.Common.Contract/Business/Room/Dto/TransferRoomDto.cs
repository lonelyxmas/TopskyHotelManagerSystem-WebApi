using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EOM.TSHotelManagement.Common.Contract
{
    public class TransferRoomDto:BaseInputDto
    {
        public string OriginalRoomNumber { get; set; }

        public string TargetRoomNumber { get; set; }

        public string CustomerNumber { get; set; }
    }
}
