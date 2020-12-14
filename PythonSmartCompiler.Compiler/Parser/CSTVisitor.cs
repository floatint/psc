using System;
using System.Collections.Generic;
using System.Text;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using PythonSmartCompiler.Compiler.Parser.AST;

namespace PythonSmartCompiler.Compiler.Parser
{
    class CSTVisitor : Python3BaseVisitor<AST.ASTNode>
    {
        public override ASTNode VisitFile_input([NotNull] Python3Parser.File_inputContext context)
        {
            
            return base.VisitFile_input(context);
        }
    }
}
