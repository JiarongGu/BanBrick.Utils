using System;
using System.Collections.Generic;
using System.Linq;

namespace BanBrick.Utils
{
    /// <summary>
     /// Service accessor interface for GetService by service name 
     /// </summary>
     /// <typeparam name="TService">interface type</typeparam>
    public interface IServiceAccessor<TService> where TService : class
    {
        TService GetService(string name);
        IEnumerable<TService> Services { get; }
    }

    /// <summary>
    /// Service accessor implementation for GetService by service name 
    /// </summary>
    /// <typeparam name="TService">interface type</typeparam>
    public class ServiceAccessor<TService> : IServiceAccessor<TService> where TService : class
    {
        private readonly IDictionary<string, Type> _serviceTypes;
        private readonly IServiceProvider _serviceProvider;
        private readonly Lazy<IEnumerable<TService>> _services;

        public IEnumerable<TService> Services => _services.Value;

        public ServiceAccessor(Dictionary<string, Type> serviceTypes, IServiceProvider serviceProvider)
        {
            _serviceTypes = serviceTypes;
            _serviceProvider = serviceProvider;

            _services = new Lazy<IEnumerable<TService>>(() =>
                _serviceTypes.Select(x => (TService)_serviceProvider.GetService(x.Value)).ToList()
            );
        }

        public TService GetService(string name)
        {
            if (_serviceTypes.ContainsKey(name))
                return (TService)_serviceProvider.GetService(_serviceTypes[name]);
            return null;
        }
    }
}
