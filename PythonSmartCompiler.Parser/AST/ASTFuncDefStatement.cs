using System;
using System.Collections.Generic;
using System.Text;

namespace PythonSmartCompiler.Parser.AST
{
    class ASTFuncDefStatement : ASTNode, IASTStatement
    {
        public ASTFuncDefStatement(string funcName)
        {
            FuncName = funcName;
        }

        public string FuncName { set; get; }
        public IList<ASTFuncParameter> Parameters { set; get; }
        public IList<IASTStatement> Statements { set; get; } = new List<IASTStatement>();

    }
}
