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
        public string GetName()
        {
            return _name;
        }
    }
    public class TargetClass
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
            //get all properties by setting flags for public and nonpublic instances
            var props = t.GetProperties(BindingFlags.Public | BindingFlags
                .NonPublic | BindingFlags.Instance);
            foreach( var prop in props )
            {
                if (prop.GetCustomAttribute<CustomNameAttribute>() != null)
                {
                    string propCustomName = prop
                        .GetCustomAttribute<CustomNameAttribute>()!
                        .GetName();
                    sb.Append($"{propCustomName} : ");
                }
                else sb.Append($"{prop.Name} : ");
                sb.Append($"{prop.GetValue(o)} ");
                if (prop.PropertyType.IsArray)
                {
                    sb.Append("{ ");
                    IEnumerable items = (IEnumerable)prop.GetValue(o, null)!;
                    foreach (var item in items)
                        sb.Append($"'{item}' ");
                    sb.Append("}");
                }
                sb.Append("| ");
            }
            return sb.ToString();
        }
        public static object StringToObject(string str)
        {
            var info = str.Split("|");
            var classInfo = info[0].Split(":");
            var t = Type.GetType(classInfo[0]);
            var values = new Object[4];
            //var props = t.GetProperties(BindingFlags.Instance | BindingFlags
            //    .NonPublic | BindingFlags.Public);
            //int j = 0;
            //foreach (var prop in props)
            //{
            //    var value = values[j];
            //    var fieldValue = prop.PropertyType.InvokeMember("Parse",
            //        BindingFlags.Instance | BindingFlags.Public | BindingFlags.InvokeMethod,
            //        null, prop, value);
            //    prop.SetValue(t, (object?)value);
            //    j++;
            //}
            values[0] = Int32.Parse(info[1].Split(":")[1]);
            values[1] = info[2].Split(":")[1].TrimStart().TrimEnd();
            values[2] = Decimal.Parse(info[3].Split(":")[1]);
            string ofChars = info[4].Split(":")[1];
            int startIndex = ofChars.IndexOf('{') + 2;
            int endIndex = ofChars.Length - 2;
            char[] array = ofChars.Substring(startIndex, endIndex - startIndex)
                .Replace(" ", "").Replace("\'","").ToCharArray();
            values[3] = array;

            var res = Activator.CreateInstance(t!,values)!;
            
            return res;
        }
        static void Main(string[] args)
        {
            object obj = new TargetClass(1, "string", 16M, new char[] {'c', 'h', 'a', 'r'});
            string strOfObj = ObjectToString(obj);
            Console.WriteLine(strOfObj);
            obj = (TargetClass)StringToObject(strOfObj);
            strOfObj = ObjectToString(obj!);
            Console.WriteLine(strOfObj);
        }
    }
}
