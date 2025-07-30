using EmailOtpProject.Services;
using EmailOtpProject.Helpers;
using EmailOtpProject.Models;
using EmailOtpProject.Interfaces;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;

namespace OTP_Module_dotnet.Tests
{
    public class EmailOtpServiceTests
    {
        private EmailOtpService CreateService(out EmailService mockEmailService)
        {
            mockEmailService = new EmailService();
            return new EmailOtpService(mockEmailService);
        }

        [Fact]
        public void GenerateOtpEmail_ValidDsoEmail_ReturnsOk()
        {
            var service = CreateService(out _);
            var result = service.GenerateOtpEmail("john@dso.org.sg");
            Assert.Equal(StatusCodes.STATUS_EMAIL_OK, result);
        }

        [Fact]
        public void GenerateOtpEmail_InvalidEmailFormat_ReturnsInvalid()
        {
            var service = CreateService(out _);
            var result = service.GenerateOtpEmail("notanemail");
            Assert.Equal(StatusCodes.STATUS_EMAIL_INVALID, result);
        }

        [Fact]
        public void GenerateOtpEmail_WrongDomain_ReturnsInvalid()
        {
            var service = CreateService(out _);
            var result = service.GenerateOtpEmail("john@gmail.com");
            Assert.Equal(StatusCodes.STATUS_EMAIL_INVALID, result);
        }

        [Fact]
        public void CheckOtp_CorrectOtpWithinTries_ReturnsOk()
        {
            var service = CreateService(out _);
            service.GenerateOtpEmail("valid@dso.org.sg");

            var otpField = typeof(EmailOtpService).GetField("_currentOTP", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            string? correctOtp = otpField?.GetValue(service) as string;
            Assert.NotNull(correctOtp);

            var input = new MockIOStream(new[] { correctOtp! });
            var result = service.CheckOtp(input);

            Assert.Equal(StatusCodes.STATUS_OTP_OK, result);
        }

        [Fact]
        public void CheckOtp_WrongOtp10Times_ReturnsFail()
        {
            var service = CreateService(out _);
            service.GenerateOtpEmail("valid@dso.org.sg");

            var input = new MockIOStream(Enumerable.Repeat("000000", 10));
            var result = service.CheckOtp(input);

            Assert.Equal(StatusCodes.STATUS_OTP_FAIL, result);
        }

        [Fact]
        public void CheckOtp_TimeoutAfter1Min_ReturnsTimeout()
        {
            var service = CreateService(out _);
            service.GenerateOtpEmail("valid@dso.org.sg");

            // Simulate no input 
            var input = new MockIOStream(new List<string>());

            var result = service.CheckOtp(input);

            Assert.Equal(StatusCodes.STATUS_OTP_TIMEOUT, result);
        }
    }

    public class MockIOStream : IOStream
    {
        private readonly Queue<string> _inputs;

        public MockIOStream(IEnumerable<string> inputs)
        {
            _inputs = new Queue<string>(inputs);
        }

        public string ReadOTP()
        {
            // Simulate delayed input 
            if (_inputs.Count == 0)
            {
                Task.Delay(TimeSpan.FromMinutes(2)).Wait(); // simulate timeout
                return string.Empty;
            }

            return _inputs.Dequeue();
        }
    }
}
