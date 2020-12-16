using System;
using PythonSmartCompiler.Compiler;

namespace PythonSmartCompiler.Driver
{
    class Program
    {
        static void Main(string[] args)
        {
            //var ass = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("lol"), AssemblyBuilderAccess.RunAndCollect);
            //var mb = ass.DefineDynamicModule("aaaa");
            ////var mb = ass.DefineDynamicModule("Test", "Test.dll", false);
            //var t = mb.DefineType("Foo", TypeAttributes.Public, typeof(ValueType));
            //t.DefineField("foo", typeof(int), FieldAttributes.Public);
            ////var metB = t.DefineMethod("main", MethodAttributes.Public);
            //t.CreateType();
            //ass.Save("Test.dll");

            Console.WriteLine("Hello World!");
            ICompiler compiler = new CLRCompiler();
            compiler.Compile("test.txt");
            // get compiler
            //var res = new Compiler.Compiler();
            //res.Compile("test.txt");
        }
    }
}
