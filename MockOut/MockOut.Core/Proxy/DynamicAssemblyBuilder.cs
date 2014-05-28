using System;
using System.Reflection;
using System.Reflection.Emit;

namespace MockOut.Core.Proxy
{
    internal static class DynamicAssemblyBuilder
    {
        public static AssemblyBuilder Create(string assemblyName)
        {
            var name = new AssemblyName(assemblyName);
            var assembly = AppDomain.CurrentDomain.DefineDynamicAssembly(
                    name,
                    AssemblyBuilderAccess.Run);

            DynamicCache.Add(assembly);

            return assembly;
        }

        public static AssemblyBuilder Create(string assemblyName, string filePath)
        {
            var name = new AssemblyName(assemblyName);
            var assembly = AppDomain.CurrentDomain.DefineDynamicAssembly(
                name,
                AssemblyBuilderAccess.RunAndSave,
                filePath);

            DynamicCache.Add(assembly);

            return assembly;
        }
    }
}
