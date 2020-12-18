using System;
using System.Collections.Generic;
using System.Text;

namespace PythonSmartCompiler.Parser.AST.Arithmetic
{
    public class ASTArithMulExpression : ASTNode, IASTArithExpression
    {
        public IASTExpression Left { set; get; }
        public IASTExpression Right { set; get; }
    }
}
