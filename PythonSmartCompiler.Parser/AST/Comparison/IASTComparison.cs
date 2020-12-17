using System;
using System.Collections.Generic;
using System.Text;

namespace PythonSmartCompiler.Parser.AST.Comparison
{
    public interface IASTComparison : IASTBinExpression
    {
        IASTExpression Left { set; get; }
        IASTExpression Right { set; get; }
    }
}
