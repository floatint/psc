using System;
using System.Collections.Generic;
using System.Text;

namespace PythonSmartCompiler.Parser.AST.Arithmetic
{
    class ASTArithFloorDivExpression : ASTNode, IASTArithExpression
    {
        public IASTExpression Left { set; get; }
        public IASTExpression Right { set; get; }
    }
}
