namespace DocsAndHospitals.Auth
{
    public class JwtSettings
    {
        public string SecretKey { get; set; } = null!;
        public string Issuer { get; set; } = "DocsAndHospitals";
        public string Audience { get; set; } = "DocsAndHospitalsClient";
        public int ExpiryMinutes { get; set; } = 60;
    }
}
