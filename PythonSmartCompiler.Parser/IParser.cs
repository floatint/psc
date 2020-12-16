using System;

namespace PythonSmartCompiler.Parser
{
    public interface IParser
    {
        AST.ASTNode Parse(string sourceCode);
    }
}
