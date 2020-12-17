using System;
using System.Collections.Generic;
using System.Text;

namespace PythonSmartCompiler.Parser.AST
{
    class ASTTestExpression : ASTNode, IASTExpression
    {
        public IASTExpression Condition { set; get; }
        public IASTExpression TrueValue { set; get; }
        public IASTExpression FalseValue { set; get; }
    }
}
