using EmailOtpProject.Interfaces;
using EmailOtpProject.Models;
using EmailOtpProject.Services;

class ConsoleIO : IOStream
{
    public string ReadOTP()
    {
        Console.Write("Enter OTP: ");
        return Console.ReadLine() ?? string.Empty;
    }
}

class Program
{
    static void Main(string[] args)
    {
        var emailService = new EmailService();
        var otpService = new EmailOtpService(emailService);

        otpService.Start();

        Console.Write("Enter your email: ");
        string email = Console.ReadLine() ?? string.Empty;

        int emailStatus = otpService.GenerateOtpEmail(email);
        Console.WriteLine($"Email status: {emailStatus}");

        if (emailStatus == StatusCodes.STATUS_EMAIL_OK)
        {
            var input = new ConsoleIO();
            int otpStatus = otpService.CheckOtp(input);
            Console.WriteLine($"OTP status: {otpStatus}");
        }

        otpService.Close();
    }
}
