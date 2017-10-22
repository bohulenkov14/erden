using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Loader;
using Microsoft.DotNet.InternalAbstractions;
using Microsoft.Extensions.DependencyModel;

namespace Erden.Core
{
    /// <summary>
    /// Auto registrator for handlers
    /// </summary>
    public sealed class AutoRegistrator
    {
        private IServiceProvider provider;

        public AutoRegistrator(IServiceProvider provider)
        {
            this.provider = provider;
        }

        /// <summary>
        /// Register all handlers
        /// </summary>
        /// <param name="handlerType">Handler type</param>
        /// <param name="registrator">Registrator type</param>
        /// <param name="method">Method in handler</param>
        public void Register(Type handlerType, Type registrator, string method)
        {
            var runtimeId = RuntimeEnvironment.GetRuntimeIdentifier();
            var assemblies = DependencyContext.Default.GetRuntimeAssemblyNames(runtimeId);
            var loadContext = AssemblyLoadContext.Default;

            List<TypeInfo> types = new List<TypeInfo>();
            foreach (var assemblyName in assemblies)
            {
                var assembly = loadContext.LoadFromAssemblyName(assemblyName);
                types.AddRange(assembly.DefinedTypes
                    .Where(p => p.GetInterfaces()
                    .Where(i => i.GetTypeInfo().IsGenericType
                        && i.GetGenericTypeDefinition() == handlerType).Count() > 0));
            }

            var registerMethod = registrator.GetMethod("Register");

            foreach (var type in types)
            {
                var interfaces = type.GetInterfaces();
                foreach (var @interface in interfaces)
                {
                    Type[] typeArguments = @interface.GenericTypeArguments;
                    MethodInfo handleMethod = type.GetMethod(method, new[] { typeArguments[0] });
                    MethodInfo genericRegister = registerMethod.MakeGenericMethod(typeArguments);

                    var parameter = Expression.Parameter(typeArguments[0]);
                    var instance = Expression.Constant(provider.GetService(type.AsType()));
                    var handleMethodCall = Expression.Call(instance, handleMethod, parameter);
                    var lambda = Expression.Lambda(handleMethodCall, parameter).Compile();

                    genericRegister.Invoke(provider.GetService(registrator), new object[] { lambda });
                }
            }
        }
    }
}