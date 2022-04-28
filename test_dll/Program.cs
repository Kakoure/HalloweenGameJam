using System;
using System.Reflection;


namespace test_dll
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("loading assembly");
            Assembly a = Assembly.LoadFile(args[0]);

            Console.WriteLine(a.FullName);
        }
    }
}
