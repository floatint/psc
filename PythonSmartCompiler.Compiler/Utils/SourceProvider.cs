using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Antlr4.Runtime;

namespace PythonSmartCompiler.Compiler.Utils
{
    class SourceProvider
    {
        public static AntlrInputStream FromFile(string sourcePath, Encoding encoding)
        {
            string sourceCode = File.ReadAllText(sourcePath, encoding);
            return new AntlrInputStream(sourceCode);
        }
    }
}
