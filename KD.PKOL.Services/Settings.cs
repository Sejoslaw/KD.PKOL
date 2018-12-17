using System.Configuration;

namespace KD.PKOL.Services
{
    public static class Settings
    {
        public static int PORT
        {
            get
            {
                string port = ConfigurationManager.AppSettings["Port"];
                int.TryParse(port, out int ret);
                return ret;
            }
        }
        public static string HOST => ConfigurationManager.AppSettings["Host"];
        public static bool SSL
        {
            get
            {
                string ssl = ConfigurationManager.AppSettings["Ssl"];
                bool.TryParse(ssl, out bool sslParsed);
                return sslParsed;
            }
        }
        public static int TIMEOUT
        {
            get
            {
                string timeout = ConfigurationManager.AppSettings["Timeout"];
                int.TryParse(timeout, out int ret);
                return ret;
            }
        }
        public static string USERNAME => ConfigurationManager.AppSettings["UserName"];
        public static string PASSWORD => ConfigurationManager.AppSettings["Password"];
        public static string EMAILFROM => ConfigurationManager.AppSettings["EmailFrom"];
    }
}