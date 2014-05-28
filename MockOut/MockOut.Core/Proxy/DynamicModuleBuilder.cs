using System.Reflection.Emit;

namespace MockOut.Core.Proxy
{
    internal static class DynamicModuleBuilder
    {
        public static ModuleBuilder Create()
        {
            var assemblyName = DynamicCache.GetAssembly.GetName().Name;

            var moduleBuilder = DynamicCache.GetAssembly.DefineDynamicModule(
                assemblyName,
                string.Format("{0}.dll", assemblyName));

            DynamicCache.Add(moduleBuilder);

            return moduleBuilder;
        }
    }
}
