using System;
using System.Collections.Generic;
using System.Text;

namespace PythonSmartCompiler.Parser.AST
{
    class ASTFuncParameter : ASTNode
    {
        public ASTFuncParameter(string name)
        {
            Name = name;
        }

        public string Name { set; get; }
        public IASTExpression Constraint { set; get; }
        public IASTExpression DefaultValue { set; get; }
        public FuncParameterTypeEnum Type { set; get; }
    }
}
