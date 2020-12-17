using System;
using System.Collections.Generic;
using System.Text;

namespace PythonSmartCompiler.Parser.AST
{
    public interface IASTBinExpression : IASTExpression
    {
        IASTExpression Left { set; get; }
        IASTExpression Right { set; get; }
    }
}
