namespace EOM.TSHotelManagement.Common.Contract
{
    public class ReadMenuInputDto : ListInputDto
    {
        public int Id { get; set; }

        public bool SearchParent { get; set; } = false;
    }
}


