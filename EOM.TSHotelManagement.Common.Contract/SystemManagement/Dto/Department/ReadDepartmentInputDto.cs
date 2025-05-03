namespace EOM.TSHotelManagement.Common.Contract
{
    public class ReadDepartmentInputDto : ListInputDto
    {
        public int Id { get; set; }
        public string DepartmentNumber { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentDescription { get; set; }
        public DateTime DepartmentCreationDate { get; set; }
        public string DepartmentLeader { get; set; }
        public string LeaderName { get; set; }
        public string ParentDepartmentNumber { get; set; }
        public string ParentDepartmentName { get; set; }
    }
}


