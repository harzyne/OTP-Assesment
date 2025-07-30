Email OTP Module – C# Console Application

Prerequisites

- [.NET SDK 8+](https://dotnet.microsoft.com/download)
- [Visual Studio Code](https://code.visualstudio.com/)
- Extensions:
  - C# by Microsoft (`ms-dotnettools.csharp`)
  - C# Dev Kit (`ms-dotnettools.csdevkit`)

Run the App

    dotnet build
    dotnet run

 Sample Run - Success
    Enter your email: alice@dso.org.sg
    [Mock Email Sent to alice@dso.org.sg]
    You OTP Code is 123456. The code is valid for 1 minute

    Enter OTP: 123456
    OTP status: 3 (STATUS_OTP_OK)


Features
Generates 6-digit OTPs
OTP is valid for 1 minute
Allows up to 10 OTP entry attempts
Only sends OTP to .dso.org.sg email addresses
Mock email system for safe testing