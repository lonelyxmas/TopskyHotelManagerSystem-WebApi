using EOM.TSHotelManagement.Common;

namespace EOM.TSHotelManagement.Contract
{
    public class ReadEnergyManagementInputDto : ListInputDto
    {
        public string? CustomerNumber { get; set; }
        public string? RoomNumber { get; set; }

        public DateOnly? StartDate { get; set; }

        public DateOnly? EndDate { get; set; }
    }
}

