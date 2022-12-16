namespace Application.Options
{
    public class JwtOptions
    {
        public List<string> Audiences { get; set; }
        public string Issuer { get; set; }
        public string SigningKey { get; set; }
    }
}