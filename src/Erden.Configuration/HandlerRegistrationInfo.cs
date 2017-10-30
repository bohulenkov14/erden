using System;

namespace Erden.Configuration
{
    /// <summary>
    /// Meta information for autoregistration
    /// </summary>
    internal sealed class HandlerRegistrationInfo
    {
        /// <summary>
        /// Handler type
        /// </summary>
        public Type HandlerType { get; set; }
        /// <summary>
        /// Registrator type
        /// </summary>
        public Type RegistratorType { get; set; }
        /// <summary>
        /// Handler method
        /// </summary>
        public string HandlerMethod { get; set; }
    }
}