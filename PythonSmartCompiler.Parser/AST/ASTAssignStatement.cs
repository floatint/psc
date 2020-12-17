using System;
using System.Collections.Generic;
using System.Text;

namespace PythonSmartCompiler.Parser.AST
{
    public class ASTAssignStatement : ASTNode, IASTStatement, IASTExpression
    {
        public IList<IASTExpression> LeftList { set; get; }
        public IList<IASTExpression> RightList { set; get; }
    }
}
