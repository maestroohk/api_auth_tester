namespace tester.Services.Interfaces
{
    public interface IEmailService
    {
        void SendPasswordResetEmail(string toEmail, string token);
    }
}
