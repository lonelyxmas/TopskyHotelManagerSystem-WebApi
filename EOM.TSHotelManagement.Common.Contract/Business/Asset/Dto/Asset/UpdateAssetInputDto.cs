namespace EOM.TSHotelManagement.Common.Contract
{
    public class UpdateAssetInputDto : BaseInputDto
    {
        public string AssetNumber { get; set; }
        public string AssetName { get; set; }
        public decimal AssetValue { get; set; }
        public string DepartmentCode { get; set; }
        public DateTime AcquisitionDate { get; set; }
        public string AssetSource { get; set; }
        public string AcquiredByEmployeeId { get; set; }
    }
}

