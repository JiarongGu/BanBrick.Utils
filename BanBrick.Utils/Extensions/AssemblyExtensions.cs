using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BanBrick.Utils.Extensions
{
    public static class AssemblyExtensions
    {
        public static IEnumerable<Type> GetLoadableTypes(this Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException(nameof(assembly));
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                return e.Types.Where(t => t != null);
            }
        }

        public static IEnumerable<Assembly> GetRelatedAssemblies(this Assembly assembly)
        {
            var assembilies = assembly.GetReferencedAssemblies().Select(Assembly.Load).ToList();

            if (!assembilies.Any(assembily => assembily.GetName() == assembily.GetName()))
                assembilies.Add(assembly);

            return assembilies;
        }

        public static IEnumerable<Type> GetRelatedLoadableTypes(this Assembly assembly)
        {
            return assembly.GetRelatedAssemblies().SelectMany(x => x.GetLoadableTypes());
        }

        public static IEnumerable<Type> GetRelatedLoadableTypes(this Assembly assembly, Type implementedInterfaceType)
        {
            return assembly.GetRelatedLoadableTypes().Where(x => x.IsImplementedInterface(implementedInterfaceType));
        }
    }
}
