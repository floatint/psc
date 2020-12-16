using System;
using System.Collections.Generic;
using System.Text;

namespace PythonSmartCompiler.Parser.AST
{
    class ASTIfStatement : ASTNode, IASTStatement
    {
        public IList<IASTStatement> TrueBody { set; get; }
        public IList<IASTStatement> ElseBody { set; get; }
    }
}
