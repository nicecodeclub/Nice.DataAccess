using Grit.Net.Common.Convert;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Grit.Net.Common.Factory
{
    public class FactoryHelper
    {
        private static IEnumerable<Type> GetClass(string assemblyName, string namespaceName)
        {
            if (!string.IsNullOrEmpty(assemblyName))
            {
                IEnumerable<Type> types = null;
                if (string.IsNullOrEmpty(namespaceName))
                    types = Assembly.Load(assemblyName).GetTypes().Where(t => t.IsClass);
                else
                    types = Assembly.Load(assemblyName).GetTypes().Where(t => t.IsClass && t.Namespace == namespaceName);
                return types;
            }
            return null;
        }

        private static IEnumerable<Type> GetClass(string namespaceName)
        {
            IEnumerable<Type> types = null;
            if (string.IsNullOrEmpty(namespaceName))
                types = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsClass);
            else
                types = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsClass && t.Namespace == namespaceName);
            return types;
        }

    }
}
