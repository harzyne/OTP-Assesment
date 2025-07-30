using System.Text.RegularExpressions;

namespace EmailOtpProject.Helpers
{
    public static class EmailValidator
    {
        public static bool IsValidEmail(string email)
        {
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern);
        }

        public static bool IsAllowedDomain(string email)
        {
            return email.EndsWith("@dso.org.sg", StringComparison.OrdinalIgnoreCase);
        }
    }
}
