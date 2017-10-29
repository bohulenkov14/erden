using System.Linq;
using System.Reflection;

namespace Erden.Domain.Infrastructure
{
    /// <summary>
    /// Extension for calling private Apply method
    /// </summary>
    public static class AggregateRootExtensions
    {
        public static void CallApply(this AggregateRoot root, object param)
        {
            var methods = root.GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance);
            var method = methods.FirstOrDefault(m =>
            {
                var parameters = m.GetParameters();
                return parameters.Length == 1 && parameters[0].ParameterType == param.GetType();
            });
            method?.Invoke(root, new object[] { param });
        }
    }
}