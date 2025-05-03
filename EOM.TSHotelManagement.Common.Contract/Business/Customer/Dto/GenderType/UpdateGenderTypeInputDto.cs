namespace EOM.TSHotelManagement.Common.Contract
{
    public class UpdateGenderTypeInputDto : BaseInputDto
    {
        public int GenderId { get; set; }
        public string GenderName { get; set; }
    }
}

