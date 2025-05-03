namespace EOM.TSHotelManagement.Common.Contract
{
    public class CreateEmployeeInputDto : BaseInputDto
    {
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public int Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Ethnicity { get; set; }
        public string PhoneNumber { get; set; }
        public string Department { get; set; }
        public string Address { get; set; }
        public string Position { get; set; }
        public int IdCardType { get; set; }
        public string IdCardNumber { get; set; }
        public string Password { get; set; }
        public DateTime HireDate { get; set; }
        public string PoliticalAffiliation { get; set; }
        public string EducationLevel { get; set; }
        public string EmailAddress { get; set; }
    }
}


