using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Antlr4.Runtime;

namespace PythonSmartCompiler.Parser.Utils
{
    class SourceProvider
    {
        public static AntlrInputStream FromString(string sourceCode)
        {
            return new AntlrInputStream(sourceCode);
        }
    }
}
