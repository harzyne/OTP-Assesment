namespace EmailOtpProject.Services
{
    public class EmailService
    {
        public bool SendEmail(string to, string body)
        {
            Console.WriteLine($"[Mock Email Sent to {to}]\n{body}");
            return true; // Simulate success
        }
    }
}
