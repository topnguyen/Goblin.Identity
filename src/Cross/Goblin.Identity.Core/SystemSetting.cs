namespace Goblin.Identity.Core
{
    public class SystemSetting
    {
        public static SystemSetting Current { get; set; }

        /// <summary>
        ///     Authorization Key to Access the Application
        /// </summary>
        /// <remarks>Use for protect Service in Public Network, empty or null for allow anonymous.</remarks>
        public string AuthorizationKey { get; set; }
    }
}