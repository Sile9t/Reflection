using System.Collections;
using System.Reflection;
using System.Text;

namespace Reflection
{
    [AttributeUsage(AttributeTargets.Property)]
    class CustomNameAttribute : Attribute 
    {
        string _name;
        public CustomNameAttribute(string name)
        {
            _name = name;
        }
    }
    class TargetClass
    {
        public int _i { get; set; }
        public string _s { get; set; }
        [CustomName("DDD")]
        private decimal _d { get; set; }
        public char[] _c { get; set; }
        public TargetClass(int i, string s, decimal d, char[] c)
        {
            _i = i;
            _s = s;
            _d = d;
            _c = c;
        }
    }
    internal class Program
    {
        public static string ObjectToString(object o)
        {
            StringBuilder sb = new StringBuilder();
            Type t = o.GetType();
            sb.Append($"{t.AssemblyQualifiedName} : ");
            sb.Append($"{t.Name} | ");
            var props = t.GetProperties();
            foreach( var prop in props )
            {
                if (prop.GetCustomAttribute<CustomNameAttribute>() != null)
                {

                }
                sb.Append($"{prop.Name} : ");
                if (prop.IsCollectible)
                {
                    sb.Append(new String(prop.GetValue(0) as char[]));
                }
                else sb.Append($"{prop.GetValue(0)}");
            }
            return sb.ToString();
        }
        static void Main(string[] args)
        {
            
        }
    }
}
