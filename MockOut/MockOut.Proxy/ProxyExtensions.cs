using MockOut.Core;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;

namespace MockOut.Proxy
{
    internal static class ProxyExtensions
    {
        public static AssemblyName BuildAssemblyName(string name)
        {
            return new AssemblyName(name);
        }
        
        public static AssemblyBuilder BuildAssembly(this AppDomain appDomain,
            AssemblyName assemblyName,
            AssemblyBuilderAccess access)
        {
            return appDomain.DefineDynamicAssembly(assemblyName, access);
        }

        public static ModuleBuilder BuildModule(this AssemblyBuilder assemblyBuilder, string name)
        {
            return assemblyBuilder.DefineDynamicModule(name);
        }

        public static TypeBuilder BuildType(this ModuleBuilder moduleBuilder, string name, TypeAttributes typeAttr)
        {
            return moduleBuilder.DefineType(name, typeAttr);
        }

        public static Type BuildInterfaceType<T, K>
            (this ModuleBuilder moduleBuilder,
            string name,
            TypeAttributes typeAttr,
            params Expression<Action<IProxyOptions>>[] options)
            where K : IMockDataStore, new()
        {

            var proxyOptions = new MockProxyOptions();

            foreach (var option in options)
            {
                var action = option.Compile();
                action(proxyOptions);
            }

            var typeBuilder = moduleBuilder.DefineType(name, typeAttr);
            typeBuilder.AddInterfaceImplementation(typeof(T));


            var methods = typeof(T).GetMethods();

            foreach (var methodInfo in methods)
            {
                var methodBuilder = typeBuilder.DefineMethod(
                    methodInfo.Name,
                    MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig | MethodAttributes.NewSlot,
                    methodInfo.ReturnType,
                    methodInfo.GetParameters().Select(p => p.ParameterType).ToArray());

          
                var ilGen = methodBuilder.GetILGenerator();


                if (methodInfo.ReturnType == typeof(void))
                {
                    ilGen.Emit(OpCodes.Ret);
                    typeBuilder.DefineMethodOverride(methodBuilder, methodInfo);
                    continue;
                }


                MethodInfo getTypeFromHandle = typeof(Type).GetMethod("GetTypeFromHandle");
                

                // Return expressions
                if (proxyOptions.ReturnValues.ContainsKey(methodInfo.Name))
                {
                    //ilGen.EmitWriteLine("Matches expression: " + methodInfo.Name);

                    var fn = proxyOptions.ReturnValues[methodInfo.Name].GetType().GetMethod("Invoke");

                    
                    ilGen.Emit(OpCodes.Ldtoken, methodInfo.ReturnType);
                    ilGen.Emit(OpCodes.Call, getTypeFromHandle);
                    ilGen.Emit(OpCodes.Callvirt, fn);                    
                    ilGen.Emit(OpCodes.Ret);
                    typeBuilder.DefineMethodOverride(methodBuilder, methodInfo);
                    continue;
                }

                ilGen.Emit(OpCodes.Ldtoken, methodInfo.ReturnType);
                ilGen.Emit(OpCodes.Call, getTypeFromHandle);

                var mockMethod = typeof(K).GetMethod(methodInfo.ReturnType.Name);

                if (mockMethod == null)
                {
                    // use method name
                    mockMethod = typeof(K).GetMethod(methodInfo.Name);
                }

                if (mockMethod != null)
                {
                    ilGen.Emit(OpCodes.Call, mockMethod);
                    //ilGen.Emit(OpCodes.Ldstr, mockMethod.Name);
                    //ilGen.EmitCall(OpCodes.Ldstr, mockMethod, new Type[] { });
                    //ilGen.Emit(OpCodes.Call, mockMethod);
                }
                else if (methodInfo.ReturnType.IsValueType)  // check for value type and construct...
                {

                    var objProperties = methodInfo.ReturnType.GetProperties();
                    var obj = Activator.CreateInstance(methodInfo.ReturnType, null);

                    foreach (var prop in objProperties)
                    {

                    }
                }
                else
                {
                    ilGen.Emit(OpCodes.Ldnull);
                }

                ilGen.Emit(OpCodes.Ret);

                typeBuilder.DefineMethodOverride(methodBuilder, methodInfo);

                //if (methodInfo.ReturnType == typeof(void))
                //{
                //    ilGen.Emit(OpCodes.Ret);
                //    continue;
                //}

                //var getMethod = typeof(Activator).GetMethod("CreateInstance", new Type[] { typeof(T) });

                //var localBuilder = ilGen.DeclareLocal(methodInfo.ReturnType);
                //ilGen.Emit(OpCodes.Ldtoken, localBuilder.LocalType);
                //ilGen.Emit(OpCodes.Call, typeof(Type).GetMethod("GetTypeFromHandle"));
                //ilGen.Emit(OpCodes.Callvirt, getMethod);
                //ilGen.Emit(OpCodes.Unbox_Any, localBuilder.LocalType);

                //ilGen.Emit(OpCodes.Ret);


            }

            Type interfaceType = typeBuilder.CreateType();

            return interfaceType;
            //var instance = Activator.CreateInstance(interfaceType);
            //return (T)instance;
        }
        public static string MethodName(
            Expression expression)
        {
            if (expression == null)
            {
                throw new ArgumentException(
                    "The expression cannot be null.");
            }

            if (expression is MemberExpression)
            {
                // Reference type property or field
                var memberExpression =
                    (MemberExpression)expression;
                return memberExpression.Member.Name;
            }

            if (expression is MethodCallExpression)
            {
                // Reference type method
                var methodCallExpression =
                    (MethodCallExpression)expression;
                return methodCallExpression.Method.Name;
            }

            if (expression is UnaryExpression)
            {
                // Property, field of method returning value type
                var unaryExpression = (UnaryExpression)expression;
                return MethodName(unaryExpression);
            }

            throw new ArgumentException("Invalid expression");
        }

        public static string MethodName(
            UnaryExpression unaryExpression)
        {
            if (unaryExpression.Operand is MethodCallExpression)
            {
                var methodExpression =
                    (MethodCallExpression)unaryExpression.Operand;
                return methodExpression.Method.Name;
            }

            return ((MemberExpression)unaryExpression.Operand)
                .Member.Name;
        }
    }
}
