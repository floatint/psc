using System;
using System.Collections.Generic;
using System.Text;

namespace PythonSmartCompiler.Parser.AST.Comparison
{
    class ASTLessComparison : ASTNode, IASTComparison
    {
        public IASTExpression Left { set; get; }
        public IASTExpression Right { set; get; }
    }
}
