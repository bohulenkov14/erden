namespace Erden.EventSourcing.Targets.EventStore
{
    /// <summary>
    /// EventStore connection settings
    /// </summary>
    public sealed class EventStoreSettings
    {
        /// <summary>
        /// IP Address
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// Port
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// Username
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// Password
        /// </summary>
        public string Password { get; set; }
    }
}