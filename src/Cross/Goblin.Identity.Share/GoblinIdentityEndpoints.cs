namespace Goblin.Identity.Share
{
    public static class GoblinIdentityEndpoints
    {
        /// <summary>
        ///     HTTP Method: POST
        /// </summary>
        public const string RegisterUser = "/users";
        
        /// <summary>
        ///     HTTP Method: GET
        /// </summary>
        public const string GetUser = "/users/{id}";
        
        /// <summary>
        ///     HTTP Method: PUT
        /// </summary>
        public const string UpdateUserProfile = "/users/{id}";
        
        /// <summary>
        ///     HTTP Method: DELETE
        /// </summary>
        public const string DeleteUser = "/users/{id}";
    }
}