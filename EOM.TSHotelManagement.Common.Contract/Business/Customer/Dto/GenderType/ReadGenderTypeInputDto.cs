namespace EOM.TSHotelManagement.Common.Contract
{
    public class ReadGenderTypeInputDto : ListInputDto
    {
        public int GenderId { get; set; }
        public string GenderName { get; set; }
    }
}

