namespace EOM.TSHotelManagement.Common.Contract
{
    public class ReadMenuOutputDto
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Title { get; set; }
        public string Path { get; set; }
        public int? Parent { get; set; }
        public string Icon { get; set; }
    }
}


