namespace CasaAPI.Models.Constants
{
    public static class ErrorConstants
    {
        public const string InternalServerError = "Internal Server Error occurred while processing request";
        public const string ValidationFailureError = "Invalid parameter(s) provided for the request";
        public const string UserNotExistsError = "The user does not exists with this Username";
        public const string InvalidCredentialsError = "Invalid credential, please try again with correct credential";
        public const string InactiveProfileError = "Your profile is in-active, please contact to administrator";
        public const string LockedProfileError = "Your profile is locked, please contact to administrator";
        public const string ExpiredSessionError = "Your session has been expired, please re-login to continue";
        public const string FileNotExistsToDownload = "File does not exists for Download";
    }
}
