namespace SelfManagement.API.Common
{
    public class EmailSetting
    {
        /* 
         
         
         */
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; }

        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public string FromEmail { get; set; } = string.Empty;
        public string FromName { get; set; } = string.Empty;
        public string TemplatePath { get; set; } = string.Empty;
    }
}
