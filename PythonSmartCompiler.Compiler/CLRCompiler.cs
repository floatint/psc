using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using PythonSmartCompiler.Parser;

namespace PythonSmartCompiler.Compiler
{
    public class CLRCompiler : ICompiler
    {
        public void Compile(string filePath)
        {
            try
            {
                IParser parser = new AntlrParser();
                var astRoot = parser.Parse(File.ReadAllText(filePath, Encoding.UTF8));
            }
            catch(IOException ex)
            {

            }
        }
    }
}
