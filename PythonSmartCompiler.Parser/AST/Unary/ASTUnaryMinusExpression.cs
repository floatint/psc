using System;
using System.Collections.Generic;
using System.Text;

namespace PythonSmartCompiler.Parser.AST.Unary
{
    public class ASTUnaryMinusExpression : IASTUnaryExpression
    {
        public IASTExpression Value { set; get; }
    }
}
