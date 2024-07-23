namespace tester
{
    public static class Constants
    {
        public const int TokenExpiryDays = 7;

        //Password Reset Configuration
        public const int PasswordResetTokenExpiryMinutes = 5;

        //Login Configuration
        public const int MaxLogMaxFailedLoginAttemptsinAttempts = 3;
        public const int AccountLockoutDurationInMinutes = 30;

        //Other Configuration
        public const bool DefaultUserActiveStatus = true;


        //Error Messages
        public const string UsernameAlreadyExistsMessage = "Username already exists";
        public const string AccountLockedMessage = "Account is locked. Please try again later";
        public const string InvalidUsernameOrPasswordMessage = "Invalid username or password.";
        public const string RegistrationErrorMessage = "An error occurred while registering the user.";
        public const string LoginErrorMessage = "An error occurred while logging in.";
        public const string ForgotPasswordErrorMessage = "An error occurred while processing forgot password request";
        public const string ResetPasswordErrorMessage = "An error occurred while resetting the password";
    }
}
