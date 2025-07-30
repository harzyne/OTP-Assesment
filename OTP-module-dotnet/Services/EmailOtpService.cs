using EmailOtpProject.Helpers;
using EmailOtpProject.Interfaces;
using EmailOtpProject.Models;

namespace EmailOtpProject.Services
{
    public class EmailOtpService
    {
        private string _currentOTP = string.Empty;
        private DateTime _otpGeneratedAt;

        private readonly EmailService _emailService;

        public EmailOtpService(EmailService emailService)
        {
            _emailService = emailService;
        }

        public void Start() { }

        public void Close() { }

        public int GenerateOtpEmail(string userEmail)
        {
            if (!EmailValidator.IsValidEmail(userEmail) || !EmailValidator.IsAllowedDomain(userEmail))
                return StatusCodes.STATUS_EMAIL_INVALID;

            _currentOTP = GenerateOtp();
            _otpGeneratedAt = DateTime.UtcNow;

            string emailBody = $"You OTP Code is {_currentOTP}. The code is valid for 1 minute";

            return _emailService.SendEmail(userEmail, emailBody)
                ? StatusCodes.STATUS_EMAIL_OK
                : StatusCodes.STATUS_EMAIL_FAIL;
        }

        public int CheckOtp(IOStream input)
        {
            const int maxAttempts = 10;
            int attempts = 0;
            TimeSpan timeout = TimeSpan.FromMinutes(1);
            CancellationTokenSource cts = new CancellationTokenSource(timeout);

            try
            {
                while (attempts < maxAttempts)
                {
                    string userOtp = Task.Run(() => input.ReadOTP(), cts.Token).Result;

                    if ((DateTime.UtcNow - _otpGeneratedAt) > timeout)
                        return StatusCodes.STATUS_OTP_TIMEOUT;

                    if (userOtp == _currentOTP)
                        return StatusCodes.STATUS_OTP_OK;

                    attempts++;
                }

                return StatusCodes.STATUS_OTP_FAIL;
            }
            catch (OperationCanceledException)
            {
                return StatusCodes.STATUS_OTP_TIMEOUT;
            }
        }

        private string GenerateOtp()
        {
            return new Random().Next(100000, 999999).ToString();
        }
    }
}
