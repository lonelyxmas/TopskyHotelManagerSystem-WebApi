using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EOM.TSHotelManagement.Common.Contract
{
    public class CheckoutRoomDto:BaseInputDto
    {
        public string RoomNumber { get; set; }
        public string CustomerNumber { get; set; }
        public decimal WaterUsage { get; set; }
        public decimal ElectricityUsage { get; set; }
    }
}
