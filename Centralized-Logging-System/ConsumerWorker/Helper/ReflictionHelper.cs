using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsumerWorker.Helper
{
    public class ReflictionHelper
    {
        public static string GetClassName<TClass>()
          => typeof(TClass).Name;

        public static string GetClassFullName<TClass>()
            => typeof(TClass).FullName;

        public static string GetCurrentExecutionDDL()
            => Assembly.GetExecutingAssembly().GetName().Name.ToLower().Replace(".", "_");

        public static object CreateInstance<TClass>()
        {
            Assembly assembly = typeof(TClass).Assembly;

            Type type = assembly.GetType(GetClassName<TClass>());
            if (type == null)
                throw new ArgumentException($" Type {type} Not Found In The Current Assembly ");
            return Activator.CreateInstance(type);
        }



    }

}
