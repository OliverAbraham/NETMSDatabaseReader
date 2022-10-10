
namespace HomenetBase
{
	public class EmailSettings
    {
        public string SmtpServer { get; set; }
        public int    SmtpPort   { get; set; }
        public bool   SmtpSSL    { get; set; }
        public string SmtpUser   { get; set; }
        public string SmtpPass   { get; set; }
        public string Absender   { get; set; }
        public string Empfänger  { get; set; }
    }
}
