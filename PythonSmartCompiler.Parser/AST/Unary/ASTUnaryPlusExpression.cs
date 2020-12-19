using System;
using System.Collections.Generic;
using System.Text;

namespace PythonSmartCompiler.Parser.AST.Unary
{
    public class ASTUnaryPlusExpression : IASTUnaryExpression
    {
        public IASTExpression Value { set; get; }
    }
}
