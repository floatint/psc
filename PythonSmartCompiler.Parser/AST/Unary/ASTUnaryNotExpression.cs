using System;
using System.Collections.Generic;
using System.Text;

namespace PythonSmartCompiler.Parser.AST.Unary
{
    public class ASTUnaryNotExpression : IASTUnaryExpression
    {
        public IASTExpression Value { set; get; }
    }
}
