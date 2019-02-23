using BanBrick.Utils.DependencyInjection;
using BanBrick.Utils.Internals;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BanBrick.Utils.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add Services based on DependencyInjection Interfaces
        /// </summary>
        public static IServiceCollection AddRegistedServices(this IServiceCollection services)
        {
            services.AddRegistedServices(typeof(ITransientDependency<>), ServiceLifetime.Transient);
            services.AddRegistedServices(typeof(IScopedDependency<>), ServiceLifetime.Scoped);
            services.AddRegistedServices(typeof(ISingletonDependency<>), ServiceLifetime.Singleton);
            return services;
        }

        private static IServiceCollection AddRegistedServices(this IServiceCollection services, Type interfaceType, ServiceLifetime serviceLifetime)
        {
            var loadableTypes = Assembly.GetEntryAssembly().GetRelatedLoadableTypes(interfaceType);
            foreach (var type in loadableTypes)
            {
                var interfaces = type.GetMatchedInterfaces(typeof(IScopedDependency<>)).ToList();
                interfaces.ForEach(i => services.Add(new ServiceDescriptor(i.GetGenericArguments()[0], type, serviceLifetime)));
            }
            return services;
        }

        /// <summary>
        /// Add Configurations based on IConfigurationDependency interface
        /// </summary>
        public static IServiceCollection AddConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddConfigurations(configuration, out var options);
        }

        /// <summary>
        /// Add Configurations based on IConfigurationDependency interface, output dictionary of config sections
        /// </summary>
        /// <param name="options">output config</param>
        public static IServiceCollection AddConfigurations(this IServiceCollection services, IConfiguration configuration, out IDictionary<string, IOptions<object>> options)
        {
            var loadableTypes = Assembly.GetEntryAssembly().GetRelatedLoadableTypes(typeof(IConfigurationDependency));

            options = new Dictionary<string, IOptions<object>>();

            foreach (var type in loadableTypes)
            {
                var section = configuration.GetSection(type.Name).Get(type);
                var serviceType = typeof(IOptions<>).MakeGenericType(type);
                var option = (IOptions<object>)Activator.CreateInstance(typeof(Options<>).MakeGenericType(type), section);
                options[type.Name] = option;
                services.AddSingleton(serviceType, option);
            }
            return services;
        }
        
        /// <summary>
        /// Add ServiceAccessor by Interfaces
        /// Used for interfaces has multiple implementations, ServiceAccessor can locate the service by its name
        /// </summary>
        public static IServiceCollection AddServiceAccessor<TService>(this IServiceCollection services, ServiceLifetime serviceLifetime)
            where TService : class
        {
            var loadableTypes = Assembly.GetEntryAssembly().GetRelatedLoadableTypes(typeof(TService));
            var serviceTypes = new Dictionary<string, Type>();

            foreach (var type in loadableTypes)
            {
                var implementationDescriptor = new ServiceDescriptor(type, type, serviceLifetime);
                services.Add(implementationDescriptor);

                if (serviceTypes.ContainsKey(type.Name))
                {
                    throw new Exception($"duplicated implementation name found {type.Name}");
                }
                serviceTypes[type.Name] = type;
            }

            var serviceAccessorDescriptor = new ServiceDescriptor(
                typeof(IServiceAccessor<TService>),
                s => new ServiceAccessor<TService>(serviceTypes, s),
                serviceLifetime
            );

            services.Add(serviceAccessorDescriptor);
            return services;
        }
    }
}
