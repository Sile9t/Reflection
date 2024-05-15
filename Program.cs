namespace Reflection
{
    [AttributeUsage(AttributeTargets.Property)]
    class CustomNameAttribute : Attribute { }
    class TargetClass
    {
        public int _i { get; set; }
        public string _s { get; set; }
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
        static void Main(string[] args)
        {
            
        }
    }
}
