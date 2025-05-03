namespace EOM.TSHotelManagement.Common.Contract
{
    public class DeleteMenuInputDto : BaseInputDto
    {
        public string Key { get; set; }
        public string Title { get; set; }
        public string Path { get; set; }
        public int? Parent { get; set; }
        public string Icon { get; set; }
    }
}


