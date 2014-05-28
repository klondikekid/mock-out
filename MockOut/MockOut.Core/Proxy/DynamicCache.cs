using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Text;

namespace MockOut.Core.Proxy
{
    internal static class DynamicCache
    {
        private static object assemblySyncRoot = new object();
        private static object moduleSyncRoot = new object();
        private static object typeSyncRoot = new object();

        internal static AssemblyBuilder AssemblyCache = null;
        internal static ModuleBuilder ModuleCache = null;

        internal static Dictionary<string, Type> TypeCache = new Dictionary<string, Type>();

        public static void Add(AssemblyBuilder assemblyBuilder)
        {
            lock (assemblySyncRoot)
            {
                AssemblyCache = assemblyBuilder;
            }
        }

        public static void Add(ModuleBuilder moduleBuilder)
        {
            lock (moduleSyncRoot)
            {
                ModuleCache = moduleBuilder;
            }
        }

        public static AssemblyBuilder GetAssembly
        {
            get
            {
                lock (assemblySyncRoot)
                {
                    if (AssemblyCache != null)
                    {
                        return AssemblyCache;
                    }
                }

                throw new Exception("Assembly not present in cache.");
            }
        }

        public static ModuleBuilder GetModule
        {
            get
            {
                lock (moduleSyncRoot)
                {
                    if (ModuleCache != null)
                    {
                        return ModuleCache;
                    }
                }

                throw new Exception("Module not present in cache.");
            }
        }

        public static void AddProxyForType(Type type, Type proxy)
        {
            lock (typeSyncRoot)
            {
                TypeCache.Add(GetHashCode(type.AssemblyQualifiedName), proxy);
            }
        }

        public static Type TryGetProxyForType(Type type)
        {
            lock (typeSyncRoot)
            {
                Type proxyType;
                TypeCache.TryGetValue(GetHashCode(type.AssemblyQualifiedName), out proxyType);
                return proxyType;
            }
        }

        private static string GetHashCode(string fullName)
        {
            SHA1CryptoServiceProvider provider = new SHA1CryptoServiceProvider();
            Byte[] buffer = Encoding.UTF8.GetBytes(fullName);
            Byte[] hash = provider.ComputeHash(buffer, 0, buffer.Length);
            return Convert.ToBase64String(hash);
        }
    }
}
