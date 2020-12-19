using System;
using System.Collections.Generic;
using System.Text;

namespace PythonSmartCompiler.Parser.AST.Logic
{
    class ASTLogicNotExpression : ASTNode, IASTLogicExpression
    {
        public IASTExpression Expression { set; get; }
    }
}
