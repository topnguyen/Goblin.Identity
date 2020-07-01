namespace Goblin.Identity.Share
{
    public static class GoblinIdentityEndpoints
    {
        /// <summary>
        ///     HTTP Method: POST
        /// </summary>
        public const string CreateSample = "/samples";
        
        /// <summary>
        ///     HTTP Method: GET
        /// </summary>
        public const string GetSample = "/samples/{id}";
        
        /// <summary>
        ///     HTTP Method: DELETE
        /// </summary>
        public const string DeleteSample = "/samples/{id}";
    }
}