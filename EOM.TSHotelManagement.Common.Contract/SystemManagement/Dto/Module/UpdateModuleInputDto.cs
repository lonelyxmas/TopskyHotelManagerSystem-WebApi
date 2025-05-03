namespace EOM.TSHotelManagement.Common.Contract
{
    public class UpdateModuleInputDto : BaseInputDto
    {
        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
        public string ModuleDescription { get; set; }
    }
}



