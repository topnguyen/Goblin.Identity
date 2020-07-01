using System;
using Elect.Core.SecurityUtils;

namespace Goblin.Identity.Service
{
    public static class PasswordHelper
    {
        public static string HashPassword(string password, DateTimeOffset passwordLastUpdatedTime)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                return null;
            }
            
            var salt = GenerateSalt(passwordLastUpdatedTime);
            
            var passwordHash = SecurityHelper.HashPassword(password, salt);

            return passwordHash;
        }
        
        public static string GenerateSalt(DateTimeOffset dateTimeOffset)
        {
            return dateTimeOffset.ToString("ddMMyyyyhhmmss");
        }
    }
}