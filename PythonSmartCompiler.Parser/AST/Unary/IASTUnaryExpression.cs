using System;
using System.Collections.Generic;
using System.Text;

namespace PythonSmartCompiler.Parser.AST.Unary
{
    public interface IASTUnaryExpression : IASTExpression
    {
        IASTExpression Value { set; get; }
    }
}
