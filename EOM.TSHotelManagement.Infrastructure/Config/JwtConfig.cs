namespace EOM.TSHotelManagement.Infrastructure
{
    public class JwtConfig
    {
        public string Key { get; set; }
        public int ExpiryMinutes { get; set; }
    }
}
