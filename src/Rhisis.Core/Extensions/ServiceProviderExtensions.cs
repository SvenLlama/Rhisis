using Microsoft.Extensions.DependencyInjection;
using System;

namespace Rhisis.Core.Extensions
{
    public static class ServiceProviderExtensions
    {
        public static TInstance CreateInstance<TInstance>(this IServiceProvider serviceProvider, params object[] parameters)
            where TInstance : class
        {
            return ActivatorUtilities.CreateInstance<TInstance>(serviceProvider, parameters);
        }
    }
}
