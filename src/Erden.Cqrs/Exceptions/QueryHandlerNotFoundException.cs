using System;

namespace Erden.Cqrs.Exceptions
{
    /// <summary>
    /// The exception that is thrown when query handler not found
    /// </summary>
    public class QueryHandlerNotFoundException : Exception
    {
        /// <summary>
        /// Initialize a new instance of the <see cref="QueryHandlerNotFoundException"/> class with the type of query which handler not found
        /// </summary>
        /// <param name="queryType">Query type</param>
        public QueryHandlerNotFoundException(Type queryType)
            : base($"Handler for query with type {queryType.Name} not found")
        { }
    }
}