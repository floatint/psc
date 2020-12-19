using System;
using System.Collections.Generic;
using System.Text;

namespace PythonSmartCompiler.Parser.AST
{
    public class ASTDict : ASTNode, IASTExpression
    {
        public IDictionary<IASTExpression, IASTExpression> Items { set; get; } = new Dictionary<IASTExpression, IASTExpression>();
    }
}
