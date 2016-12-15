using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace MyTestRunner
{
    public class MyTestRunner
    {
        public IEnumerable<MethodInfo> ResolveTestMethods(Type type)
        {
            return type
                .GetTypeInfo()
                .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(m => m.GetCustomAttributes<FactAttribute>().Any());
        }

        public List<MyTestRunnerResult> ExecuteTestMethods(IEnumerable<MethodInfo> methods, object instance)
        {
            var results = new List<MyTestRunnerResult>();

            foreach (var method in methods)
            {
                try
                {
                    method.Invoke(instance, new object[0]);
                }
                catch
                {
                    results.Add(new MyTestRunnerResult() { type = method, Result = false });
                }
                
            }
            return results;
        }
    }
}
