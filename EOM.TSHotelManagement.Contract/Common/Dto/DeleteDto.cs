using System.Collections.Generic;

namespace EOM.TSHotelManagement.Contract
{
    public abstract class DeleteDto
    {
        public List<int> DelIds { get; set; }
    }
}