namespace EmailOtpProject.Models
{
    public static class StatusCodes
    {
        public const int STATUS_EMAIL_OK = 0;
        public const int STATUS_EMAIL_FAIL = 1;
        public const int STATUS_EMAIL_INVALID = 2;

        public const int STATUS_OTP_OK = 3;
        public const int STATUS_OTP_FAIL = 4;
        public const int STATUS_OTP_TIMEOUT = 5;
    }
}
