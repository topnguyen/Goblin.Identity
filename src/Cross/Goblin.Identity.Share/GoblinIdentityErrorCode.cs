using Goblin.Core.Errors;

namespace Goblin.Identity.Share
{
    public class GoblinIdentityErrorCode : GoblinErrorCode
    {
        public const string UserNotFound = "User does not exist";
        
        public const string WrongPassword = "Wrong Password";
        
        public const string EmailNotUnique = "Email already exists";
        
        public const string UserNameNotUnique = "UserName already exists";
        
        public const string ConfirmEmailTokenInCorrect = "Confirm Email Token is incorrect!";
        
        public const string ConfirmEmailTokenExpired = "Confirm Email Token is expired";
        
        public const string SetPasswordTokenInCorrect = "Set Passord Token is incorrect!";

        public const string SetPasswordTokenExpired = "Set Passord Token is expired";
        
        public const string AccessTokenIsInvalid = "Access Token is invalid";
        
        public const string AccessTokenIsExpired = "Access Token is expired";
        
        public const string AccessTokenIsRevoked = "Access Token is revoked";
    }
}