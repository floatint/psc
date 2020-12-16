using System;
using System.Collections.Generic;
using System.Text;

namespace PythonSmartCompiler.Parser.AST
{
    public class ASTNode
    {
        public virtual IList<ASTNode> Children { get; }
    }
}
