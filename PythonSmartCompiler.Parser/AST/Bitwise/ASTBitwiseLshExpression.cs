using System;
using System.Collections.Generic;
using System.Text;

namespace PythonSmartCompiler.Parser.AST.Bitwise
{
    class ASTBitwiseLshExpression : ASTNode, IASTBitwiseExpression
    {
        public IASTExpression Left { set; get; }
        public IASTExpression Right { set; get; }
    }
}
