using System;
using System.Collections.Generic;
using System.Text;

namespace PythonSmartCompiler.Parser.AST
{
    class ASTCompilationUnit : ASTNode
    {
        public IList<IASTStatement> Statements { set; get; } = new List<IASTStatement>();
    }
}
