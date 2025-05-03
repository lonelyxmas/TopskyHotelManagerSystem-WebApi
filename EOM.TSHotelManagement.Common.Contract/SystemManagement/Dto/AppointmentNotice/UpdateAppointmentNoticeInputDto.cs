namespace EOM.TSHotelManagement.Common.Contract
{
    public class UpdateAppointmentNoticeInputDto : BaseInputDto
    {
        public string NoticeId { get; set; }
        public string NoticeTitle { get; set; }
        public string NoticeContent { get; set; }
        public DateTime NoticeDate { get; set; }
    }
}


