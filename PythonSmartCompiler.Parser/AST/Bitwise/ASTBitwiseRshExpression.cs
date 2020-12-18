using System;
using System.Collections.Generic;
using System.Text;

namespace PythonSmartCompiler.Parser.AST.Bitwise
{
    class ASTBitwiseRshExpression : ASTNode, IASTBitwiseExpression
    {
        public IASTExpression Left { set; get; }
        public IASTExpression Right { set; get; }
    }
}
