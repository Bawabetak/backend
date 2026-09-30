namespace bawabetak_backend.Helpers.Static
{
    public static class ResponseMesages
    {
        public static readonly Dictionary<ResponseKeys, string> Messages = new Dictionary<ResponseKeys, string>
        {
            [ResponseKeys.InternalServerError] = "Internal server error.",
            [ResponseKeys.TooManyRequests] = "Too many requests. Please try again later.",
            [ResponseKeys.success] = "Request processed successfully.",
            [ResponseKeys.FileIsRequired] = "File is required.",
            [ResponseKeys.FileSizeExceeded] = "File size exceeded the limit.",
            [ResponseKeys.InvalidFileType] = "Invalid file type.",
            [ResponseKeys.InvalidFileContentType] = "Invalid file content type.",
            [ResponseKeys.UserNotFound] = "User not found.",
            [ResponseKeys.StrategyNotFound] = "Strategy not found.",
            [ResponseKeys.InvalidVerificationCode] = "Invalid verification code.",
            [ResponseKeys.EmailAlreadyExists] = "Email already exists.",
            [ResponseKeys.RegistrationFailed] = "Registration failed.",
            [ResponseKeys.RoleAssignmentFailed] = "Role assignment failed.",
            [ResponseKeys.WrongPassword] = "Wrong password.",
            [ResponseKeys.ResetNotApproved] = "Password reset not approved.",
            [ResponseKeys.InvalidOldPassword] = "Invalid old password.",
            [ResponseKeys.PasswordChangeFailed] = "Password change failed.",
            [ResponseKeys.EmailNotVerified] = "Email not verified.",
            [ResponseKeys.InvalidEmailOrPassword] = "Invalid email or password.",
            [ResponseKeys.InvalidRefreshToken] = "Invalid refresh token.",
            [ResponseKeys.RefreshTokenReused]= " Refresh token has been reused. Please login again.",
            [ResponseKeys.RefreshTokenExpired]= "Refresh token has expired. Please login again.",
            [ResponseKeys.SendVerificationSuccess] = "Verification code sent successfully.",
            [ResponseKeys.CodeVerifiedSuccessfully] = "Verification code verified successfully.",
            [ResponseKeys.UserRegisteredSuccessfully] = "User registered successfully. check your email to verify it",
            [ResponseKeys.UserLoggedInSuccessfully] = "User logged in successfully.",
            [ResponseKeys.passwordResetSuccess] = "Password reset successfully.",
            [ResponseKeys.passwordChangeSuccess] = "Password changed successfully.",
            [ResponseKeys.TokenRefreshedSuccessfully] = "Token refreshed successfully.",
            [ResponseKeys.NewPasswordCannotBeSameAsOld] = "New password cannot be the same as the old password.",
            [ResponseKeys.RegistrationAlreadyCompleted] = "Registration has already been completed.",
            [ResponseKeys.UserCompleteRegisterSuccessfully] = "User registration completed successfully.",
            [ResponseKeys.Unauthorized] = "Unauthorized access.",
            [ResponseKeys.UserDeleteFailed] = "User deletion failed.",
            [ResponseKeys.UserDeleteSuccess] = "User deleted successfully.",
            [ResponseKeys.notApprovedToCompleteRegister] = "User is not approved to complete registration.",






        };
        public static string GetMessage(ResponseKeys key)
        {
            return Messages.ContainsKey(key) ? Messages[key] : "Unknown response key.";
        }
    }
}
