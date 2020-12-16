using System;
using System.Collections.Generic;
using System.Text;

namespace PythonSmartCompiler.Compiler
{
    public interface ICompiler
    {
        void Compile(string filePath);
    }
}
