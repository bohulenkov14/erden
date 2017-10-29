namespace Erden.EventSourcing
{
    /// <summary>
    /// Versions of entity
    /// </summary>
    public enum Version
    {
        /// <summary>
        /// Any version
        /// </summary>
        Any = -2,
        /// <summary>
        /// Last version
        /// </summary>
        Last = -2,
        /// <summary>
        /// Not existing entity
        /// </summary>
        Empty = -1
    }
}