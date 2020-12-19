using System;
using System.Collections.Generic;
using System.Text;

namespace PythonSmartCompiler.Parser.AST.Logic
{
    class ASTLogicAndExpression : ASTNode, IASTBinExpression, IASTLogicExpression
    {
        public IASTExpression Left { set; get; }
        public IASTExpression Right { set; get; }
    }
}
