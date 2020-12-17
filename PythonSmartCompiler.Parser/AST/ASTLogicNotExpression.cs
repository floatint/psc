using System;
using System.Collections.Generic;
using System.Text;

namespace PythonSmartCompiler.Parser.AST
{
    class ASTLogicNotExpression : ASTNode, IASTExpression
    {
        public IASTExpression Expression { set; get; }
    }
}
