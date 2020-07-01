using Goblin.Core.Errors;

namespace Goblin.Identity.Share
{
    public class GoblinIdentityErrorCode : GoblinErrorCode
    {
        public const string UserNotFound = "User does not exist";
        
        public const string WrongPassword = "Wrong Password";
        
        public const string EmailNotUnique = "Email already exists";
        
        public const string UserNameNotUnique = "UserName already exists";
    }
}